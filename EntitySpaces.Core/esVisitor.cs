/*  New BSD License
-------------------------------------------------------------------------------
Copyright (c) 2006-2012, EntitySpaces, LLC
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the EntitySpaces, LLC nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL EntitySpaces, LLC BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
-------------------------------------------------------------------------------
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace EntitySpaces.Core
{
    public interface IVisitable
    {
        esVisitableNode[] GetGraph(object state);
    }

    /// <summary>
    /// Indicated whether a node being visited is an Entity or a Collection
    /// </summary>
    public enum esVisitableNodeType
    {
        /// <summary>
        /// Unassigned - Should never ocurr
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// This object being visited is a Collection
        /// </summary>
        Collection = 1,
        /// <summary>
        /// The object being visited is an Entity
        /// </summary>
        Entity = 2
    }

    /// <summary>
    /// Contains information about the Node being visited
    /// </summary>
    public sealed class esVisitableNode
    {
        internal esVisitableNode()
        {

        }

        internal esVisitableNode(object o)
        {
            Obj = o;
        }

        /// <summary>
        /// The type of node, Collection or Entity
        /// </summary>
        public esVisitableNodeType NodeType
        {
            get { return nodeType; }
        }

        /// <summary>
        /// If NodeType is esVisitableNodeType.Collection then this property is valid
        /// </summary>
        public esEntityCollectionBase Collection
        {
            get
            {
                return (esEntityCollectionBase)obj;
            }
        }

        /// <summary>
        /// If NodeType is esVisitableNodeType.Entity then this property is valid
        /// </summary>
        public esEntity Entity
        {
            get
            {
                return (esEntity)obj;
            }
        }

        /// <summary>
        /// If the object being visited is a hierarchical property this is the name
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// You can use this during visitation to attach user state to this node
        /// </summary>
        public object UserState { get; set; }

        /// <summary>
        /// Allows you to set a property in the object graph to null, this is used
        /// by the PruneGraph() method.
        /// </summary>
        /// <param name="parent">The parent of this node</param>
        public void SetValueToNull(object parent)
        {
            if (parent != null)
            {
                fieldInfo.SetValue(parent, null);
            }
        } 

        internal object Obj
        {
            get { return obj; }
            set
            {
                obj = value;
                if (obj == null)
                {
                    nodeType = esVisitableNodeType.Unassigned;
                }
                else
                {
                    nodeType = (obj is esEntity) ? esVisitableNodeType.Entity : esVisitableNodeType.Collection;
                }
            }
        }


        private object obj;
        private esVisitableNodeType nodeType;
        internal FieldInfo fieldInfo;
    }

    /// <summary>
    /// Passed to each callback method during visitation. Contains everything you need
    /// to implement your logic.
    /// </summary>
    public sealed class esVisitParameters
    {
        /// <summary>
        /// The root node passed into static Visit() method, can be a Collection or an Entity
        /// </summary>
        public object Root;
        /// <summary>
        /// The parent of the "Node" being visited, can be a collection or an entity
        /// </summary>
        public esVisitableNode Parent;
        /// <summary>
        /// The "Node being visited, can be either a collection or an entity
        /// </summary>
        public esVisitableNode Node;
        /// <summary>
        /// You can store data here and it will passed throughout the visitation process.
        /// </summary>
        public object UserState;
        /// <summary>
        /// Set to false and the current nodes children will not be visited. This does not
        /// however terminate the entire visit process.
        /// </summary>
        public bool ProcessChildren;
    }

    /// <summary>
    /// This class can be used to visit the EntitySpaces hierarchical object graph. It will
    /// not cause any data to be Lazy Loaded. The esVisitor class is used internally by 
    /// the EntitySpaces code to implement methods such as IsGraphDirty, AcceptChangesGraph
    /// and so on.
    /// </summary>
    public sealed class esVisitor
    {
        // The signature for your esVisitor callback methods. If you return false the visitor stops.
        public delegate bool VisitCallback(esVisitParameters parameters);

        private VisitCallback enterCallback = null;
        private VisitCallback exitCallback = null;
        private IList<Object> references = new List<Object>();

        #region Constructors

        private esVisitor() : this(null) { }

        private esVisitor(VisitCallback callback)
        {
            enterCallback = callback;
        }

        private esVisitor(VisitCallback enterCallback, VisitCallback exitCallback)
        {
            this.enterCallback = enterCallback;
            this.exitCallback = exitCallback;
        }

        /// <summary>
        /// This method will visit each object once
        /// </summary>
        /// <param name="root">Pass in a collection or an entity</param>
        /// <param name="callback">Your callback method</param>
        /// <returns></returns>
        public static bool Visit(object root, VisitCallback callback)
        {
            return new esVisitor(callback).Visit(root, (object)null);
        }

        /// <summary>
        /// This method will visit objects in a sandwich mode, EnterCallback, operate on children, ExitCallback.
        /// </summary>
        /// <param name="root">Pass in a collection or an entity</param>
        /// <param name="enterCallback">The enter callback method</param>
        /// <param name="exitCallback">The exit callback method</param>
        /// <returns></returns>
        public static bool Visit(object root, VisitCallback enterCallback, VisitCallback exitCallback)
        {
            return new esVisitor(enterCallback, exitCallback).Visit(root, (object)null);
        }

        /// <summary>
        /// This method will visit each object once
        /// </summary>
        /// <param name="root">Pass in a collection or an entity</param>
        /// <param name="callback">Your callback method</param>
        /// <param name="userState">Pass in anything you like, it will be available to your callback method</param>
        /// <returns></returns>
        public static bool Visit(object root, VisitCallback callback, object userState)
        {
            return new esVisitor(callback).Visit(root, userState);
        }

        /// <summary>
        /// This method will visit objects in a sandwich mode, EnterCallback, operate on children, ExitCallback.
        /// </summary>
        /// <param name="root">Pass in a collection or an entity</param>
        /// <param name="enterCallback">The enter callback method</param>
        /// <param name="exitCallback">The exit callback method</param>
        /// <param name="userState">Pass in anything you like, it will be available to your callback methods</param>
        /// <returns></returns>
        public static bool Visit(object root, VisitCallback enterCallback, VisitCallback exitCallback, object userState)
        {
            return new esVisitor(enterCallback, exitCallback).Visit(root, userState);
        }

        #endregion

        #region Methods

        private bool AddIfNewReference(esVisitableNode node)
        {
            for (int i = 0; i < references.Count; i++)
            {
                if (Object.ReferenceEquals(node.Obj, references[i]))
                {
                    // existing reference
                    return false;
                }
            }

            // add at beginning of list assuming that more recent items will collide first
            references.Insert(0, node.Obj);
            return true;
        }

        private bool VisitNode(esVisitParameters p)
        {
            bool keepGoing = true;

            if (p.Node != null && AddIfNewReference(p.Node))
            {
                if (!enterCallback(p))
                {
                    keepGoing = false;
                }

                if (p.ProcessChildren && keepGoing && !VisitNodeReferences(p))
                {
                    keepGoing = false;
                }

                if (exitCallback != null && !exitCallback(p))
                {
                    keepGoing = false;
                }
            }

            return keepGoing;
        }

        private bool VisitNodeReferences(esVisitParameters p)
        {
            Type type = p.Node.Obj.GetType();

            esVisitableNode parent = p.Parent;
            esVisitableNode node = p.Node;

            p.Node = new esVisitableNode();
            p.Parent = new esVisitableNode();

            try
            {
                if (!typeof(String).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type))
                {
                    p.Parent = node;

                    int i = 0;
                    foreach (object item in (IEnumerable)node.Obj)
                    {
                        p.Node.Obj = item;
                        p.Node.PropertyName = "";
                        p.ProcessChildren = true;

                        if (!VisitNode(p))
                        {
                            return false;
                        }
                        i++;
                    }
                }

                if (typeof(IVisitable).IsAssignableFrom(type))
                {
                    esVisitableNode[] nodes = ((IVisitable)node.Obj).GetGraph(enterCallback);
                    if (nodes != null)
                    {
                        p.Parent = node;

                        for (int i = 0; i < nodes.Length; i++)
                        {
                            p.Node = nodes[i];

                            p.ProcessChildren = true;
                            if (!VisitNode(p))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            finally
            {
                p.Parent = parent;
                p.Node = node;
            }

            return true;
        }

        private bool Visit(object root, object userState)
        {
            return Visit(root, userState, true);
        }

        private bool Visit(object root, object userState, bool clearReferences)
        {
            if (clearReferences)
            {
                references.Clear();
            }

            esVisitParameters parameters = new esVisitParameters()
            {
                Root = root,
                ProcessChildren = true,
                Node = new esVisitableNode(root),
                Parent = new esVisitableNode(),
                UserState = userState
            };

            return VisitNode(parameters);
        }

        #endregion
    }
}
