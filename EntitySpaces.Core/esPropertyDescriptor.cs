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
#if (LINQ)
using System.Linq;
#endif
using System.Text;

using System.ComponentModel;

namespace EntitySpaces.Core
{
    /// <summary>
    /// Used when DataBinding internally by EntitySpaces
    /// </summary>
    public delegate object DynamicGetValue(object component);

    /// <summary>
    /// Used when DataBinding internally by EntitySpaces
    /// </summary>
    public delegate void DynamicSetValue(object component, object newValue);

    /// <summary>
    /// Used when DataBinding internally by EntitySpaces
    /// </summary>
    public class esPropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor _trueDescriptor;
        private esEntity _entity;
        private esEntity _containedEntity;
        protected Type m_componentType;
        protected Type m_propertyType;
        protected DynamicGetValue m_getDelegate;
        protected DynamicSetValue m_setDelegate;

        public esPropertyDescriptor(Type componentType, string name, Type propertyType, DynamicGetValue getDelegate, DynamicSetValue setDelegate)
            : base(name, null)
        {
            m_componentType = componentType;
            m_propertyType = propertyType;
            m_getDelegate = getDelegate;
            m_setDelegate = setDelegate;
        }

        public esPropertyDescriptor(esEntity entity, string name, Type propertyType, esEntity containedEntity)
            : base(name, null)
        {
            this._entity = entity;
            this.m_propertyType = propertyType;
            this._containedEntity = containedEntity;
        }

        public esPropertyDescriptor(esEntity entity, string name, Type propertyType)
            : base(name, null)
        {
            this._entity = entity;
            this.m_propertyType = propertyType;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                if (this._containedEntity != null)
                {
                    return this._containedEntity.GetType();
                }
                else
                {
                    return m_propertyType;
                }
            }
        }

        public override bool IsReadOnly
        {
            get { return m_setDelegate == null; }
        }

        public override Type PropertyType
        {
            get { return m_propertyType; }
        }

        public override void ResetValue(object component)
        {

        }

        public override object GetValue(object component)
        {
            esEntity obj = component as esEntity;

            if (obj == null) return null;

            if (_trueDescriptor != null)
            {
                return this._trueDescriptor.GetValue(component);
            }
            else
            {
                object o = null;

                if (m_getDelegate != null)
                {
                    o = m_getDelegate(component);
                }
                else
                {
                    obj.currentValues.TryGetValue(this.Name, out o);
                }

                return o;
            }
        }

        public override void SetValue(object component, object value)
        {
            esEntity obj = component as esEntity;

            if (obj == null) return;

            if (_trueDescriptor != null)
            {
                _trueDescriptor.SetValue(component, value);
            }
            else
            {
                if (obj != null)
                {
                    if (m_setDelegate != null)
                    {
                        m_setDelegate(obj, value);
                    }
                    else
                    {
                        obj.currentValues[this.Name] = value;
                        obj.OnPropertyChanged(this.Name);
                    }
                }
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public esEntity ContainedEntity
        {
            get { return _containedEntity; }
        }

        public PropertyDescriptor TrueDescriptor
        {
            set { this._trueDescriptor = value; }
        }
    }
}
