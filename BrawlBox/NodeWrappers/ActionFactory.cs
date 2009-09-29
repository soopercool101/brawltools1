using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.Reflection;

namespace BrawlBox
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    sealed class NodeActionAttribute : Attribute
    {
        private MethodInfo _method;
        public MethodInfo MethodInfo { get { return _method; } }

        private string _text;
        public string Text { get { return _text; } }

        public NodeActionAttribute(string text) { _text = text; }

        public Keys ShortcutKeys { get; set; }
        public string ToolTipText { get; set; }
        public bool ChildFunction { get; set; }

        private static Dictionary<Type, List<NodeActionAttribute>> _actions;
        public static Dictionary<Type, List<NodeActionAttribute>> Actions
        {
            get
            {
                if (_actions == null)
                {
                    _actions = new Dictionary<Type, List<NodeActionAttribute>>();
                    NodeActionAttribute attr;
                    foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                        foreach (MethodInfo info in t.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                            if ((attr = info.GetAttribute<NodeActionAttribute>()) != null)
                            {
                                if (!_actions.ContainsKey(t))
                                    _actions[t] = new List<NodeActionAttribute>();
                                attr._method = info;
                                _actions[t].Add(attr);
                            }
                }
                return _actions;
            }
        }
    }

    //static class ActionFactory
    //{
    //    public static ToolStripItem[] Build(TreeNode target)
    //    {
    //        if (!_actions.ContainsKey(target.GetType())) return new ToolStripItem[] { };

    //        List<NodeActionAttribute> list = _actions[target.GetType()];
    //        List<ToolStripItem> items = new List<ToolStripItem>();

    //        for (int i = 0; i < list.Count; i++)
    //        {
    //            NodeActionAttribute attr = list[i];
    //            if ((target.Parent == null) && (attr.ChildFunction))
    //                continue;

    //            ToolStripItem item = new ToolStripMenuItem(attr.Text, null, new ActionSink(target, attr.MethodInfo).OnClick, attr.ShortcutKeys);
    //            item.ToolTipText = attr.ToolTipText;

    //            items.Add(item);
    //        }

    //        return items.ToArray();
    //    }

    //    private struct ActionSink
    //    {
    //        private object _target;
    //        private MethodInfo _info;
    //        private object[] _args;

    //        public ActionSink(object target, MethodInfo info)
    //        {
    //            _target = target;
    //            _info = info;
    //            _args = new object[_info.GetParameters().Length];
    //        }

    //        public void OnClick(object sender, EventArgs e)
    //        {
    //            _info.Invoke(_target, _args);
    //        }
    //    }
    //}

    class ActionFactory
    {
        private static List<ToolStripMenuItem> _menuCache = new List<ToolStripMenuItem>();

        public static ToolStripMenuItem[] Build(BaseWrapper wrapper)
        {
            if (!NodeActionAttribute.Actions.ContainsKey(wrapper.GetType()))
                return new ToolStripMenuItem[] { };

            List<NodeActionAttribute> list = NodeActionAttribute.Actions[wrapper.GetType()];
            ToolStripMenuItem[] items = new ToolStripMenuItem[list.Count];

            for (int i = 0, x = 0; i < list.Count; i++)
            {
                NodeActionAttribute attr = list[i];
                if ((attr.ChildFunction) && (wrapper.Parent == null))
                    break;

                ToolStripMenuItem item = null;
                while (item == null)
                {
                    if (x == _menuCache.Count)
                        _menuCache.Add(item = ActionHandler.Create());
                    else if (_menuCache[x++].Owner == null)
                        item = _menuCache[x - 1];
                }
                items[i] = item;
                ActionHandler.Link(item, attr, wrapper);
            }
            return items;
        }

        class ActionHandler
        {
            private ToolStripMenuItem _item;
            private MethodInfo _method;
            private object _target;

            private ActionHandler() { _item = new ToolStripMenuItem("", null, OnClick); _item.Tag = this; }
            public static ToolStripMenuItem Create() { return new ActionHandler()._item; }

            public static void Link(ToolStripMenuItem item, NodeActionAttribute info, object target)
            {
                item.Text = info.Text;
                item.ShortcutKeys = info.ShortcutKeys;
                item.ToolTipText = info.ToolTipText;

                ActionHandler h = item.Tag as ActionHandler;
                h._method = info.MethodInfo;
                h._target = target;
            }

            public void OnClick(object sender, EventArgs e)
            {
                _method.Invoke(_target, null);
            }
        }
    }
}
