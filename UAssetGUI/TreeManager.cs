using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;

namespace UAssetGUI
{
    public partial class TreeManager : Form
    {
        public TreeManager()
        {
            InitializeComponent();
        }
        public UAsset asset;

        public void RefreshTheme()
        {
            this.ForeColor = UAGPalette.ForeColor;
            this.BackColor = UAGPalette.BackColor;

            this.ImportTree.ForeColor = UAGPalette.ForeColor;
            this.ImportTree.BackColor = UAGPalette.BackColor;
            this.ExportTree.ForeColor = UAGPalette.ForeColor;
            this.ExportTree.BackColor = UAGPalette.BackColor;

            this.ExportTab.ForeColor = UAGPalette.ForeColor;
            this.ExportTab.BackColor = UAGPalette.BackColor;
            this.ImportTab.ForeColor = UAGPalette.ForeColor;
            this.ImportTab.BackColor = UAGPalette.BackColor;
        }
        private void TreeManager_Load(object sender, EventArgs e)
        {
            RefreshTheme();
            RefreshTrees();
        }
        public void RefreshTrees()
        {
            ImportTree.Nodes.Clear();
            ExportTree.Nodes.Clear();
            if (asset == null) return;
            List<Import> addedImports = new List<Import>();
            TreeNode importToTreeNode(Import a)
            {
                Trace.Assert(!addedImports.Contains(a), "Import already Added");
                addedImports.Add(a);
                TreeNode node = new($"{a.ClassName.Value.Value} {a.ObjectName.Value.Value}");
                node.Tag = a;
                var index = -asset.Imports.FindIndex(x => x == a) - 1;
                node.Nodes.AddRange(asset.Imports.Where(x => x.OuterIndex.Index == index).Select(x => importToTreeNode(x)).ToArray());
                return node;
            }
            foreach (var import in asset.Imports)
            {
                if (import.OuterIndex.Index != 0 || import.ClassName.Value.Value != "Package") continue;
                string path = import.ObjectName.Value.Value;
                var pathList = path.Split("/", StringSplitOptions.RemoveEmptyEntries);
                TreeNode currentNode = null;
                for (int i = 0; i < pathList.Length; i++)
                {
                    TreeNodeCollection searchNodes = i == 0 ? ImportTree.Nodes : currentNode.Nodes;
                    if (i != pathList.Length-1)
                    {
                        var Node = searchNodes.ContainsKey(pathList[i]) ? searchNodes.Find(pathList[i], false).Single() : null;
                        currentNode = Node ?? searchNodes.Add(pathList[i], $"/{pathList[i]}");
                    }
                    else
                    {
                        var newNode = importToTreeNode(import);
                        newNode.Name = pathList[i];
                        newNode.Tag = import;
                        newNode.Text = $"Package {pathList[i]}";
                        currentNode.Nodes.Add(newNode);
                    }
                }
                //ImportTree.Nodes.Add(importToTreeNode(import));
            }
            if (addedImports.Count != asset.Imports.Count)
            {
                var difference = asset.Imports.Except(addedImports);
                TreeNode orphanNode = new("Orphaned Imports");
                foreach (var import in difference)
                {
                    if(!addedImports.Contains(import))
                    {
                        orphanNode.Nodes.Add(importToTreeNode(import));
                    }
                }
                ImportTree.Nodes.Add(orphanNode);
            }
            List<Export> addedExports = new List<Export>();
            TreeNode exportToTreeNode(Export a)
            {
                TreeNode node = new($"{(a.ClassIndex.IsImport() ? a.ClassIndex.ToImport(asset).ObjectName : a.ClassIndex.ToExport(asset).ObjectName).Value.Value} {a.ObjectName.Value.Value}");
                if (!addedExports.Contains(a))
                {
                    addedExports.Add(a);
                }
                else
                {
                    node.Text += $" [Duplicate] {addedExports.FindAll( x => x == a).Count()}";
                    addedExports.Add(a);
                }
                //Trace.Assert(!addedExports.Contains(a), "Export already added to Tree");
                //addedExports.Add(a);

                node.Tag = a;
                var index = asset.Exports.FindIndex(x => x == a) + 1;
                if (a is StructExport c)
                {
                    var addedExports = c.Children.Select(x => x.ToExport(asset));
                    node.Nodes.AddRange(addedExports.Select(x => exportToTreeNode(x)).ToArray());
                    node.Nodes.AddRange(asset.Exports.Where(x => x.OuterIndex.Index == index).Except(addedExports).Select(x => exportToTreeNode(x)).ToArray());
                }
                else if(a is NormalExport b)
                {
                    var addedObjectExports = b.Data.Where(x => x.PropertyType.Value == "ObjectProperty" && ((ObjectPropertyData)x).Value.IsExport() && ((ObjectPropertyData)x).Value.ToExport(asset).OuterIndex.Index == index).Select(x => (Property:x, Data:((ObjectPropertyData)x).Value.ToExport(asset))).ToArray();
                    node.Nodes.AddRange(b.Data.Select(x => new TreeNode($"{x.Name.Value.Value} {x.PropertyType.Value}", addedObjectExports.Select(x => x.Property).Contains(x) ? new TreeNode[] {exportToTreeNode(((ObjectPropertyData)x).Value.ToExport(asset))} : x.PropertyType.Value == "StructProperty" ? ((StructPropertyData)x).Value.Select(x => new TreeNode($"{x.PropertyType.Value} {x.Name}")).ToArray() : new TreeNode[] { } )).ToArray());
                    node.Nodes.AddRange(asset.Exports.Where(x => x.OuterIndex.Index == index).Except(addedObjectExports.Select(x => x.Data)).Select(x => exportToTreeNode(x)).ToArray());
                }
                else
                {
                    node.Nodes.AddRange(asset.Exports.Where(x => x.OuterIndex.Index == index).Select(x => exportToTreeNode(x)).ToArray());
                }

                    return node;
            }
            foreach (var export in asset.Exports)
            {
                if (export.OuterIndex.Index != 0) continue;
                //TreeNode node = new($"{export.GetExportClassType().Value.Value} {export.ObjectName.Value.Value}");
                ExportTree.Nodes.Add(exportToTreeNode(export));
            }
            if (addedExports.Distinct().Count() != asset.Exports.Count)
            {
                Debug.WriteLine($"{addedExports.Distinct().Count()} {asset.Exports.Count}");
                var orphanNode = ExportTree.Nodes.Add("Orphaned Exports");
                var exportDifference = asset.Exports.Except(addedExports);
                foreach (var export in exportDifference)
                {
                    if (addedExports.Contains(export)) continue;
                    orphanNode.Nodes.Add(exportToTreeNode(export));
                }
            }
        }

        private void ImportTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void ExportTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (Application.OpenForms[0] is Form1 frm1)
            {
                TreeNode exportNode = e.Node;
                var exportIndex = asset.Exports.FindIndex(x => x == exportNode.Tag);
                while (exportIndex == -1)
                {
                    if (exportNode == null) return;
                    exportNode = exportNode.Parent;
                    if (exportNode.Tag != null)
                    {
                        exportIndex = asset.Exports.FindIndex(x => x == exportNode.Tag);
                    }
                    if (exportNode == null) return;
                }
                frm1.treeView1.SelectedNode = frm1.treeView1.Nodes[frm1.treeView1.Nodes.Count - 1].Nodes[exportIndex].Nodes[0];
            }
        }
    }
}
