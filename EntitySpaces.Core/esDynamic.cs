using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace EntitySpaces.Core
{
    public class esDynamic : DynamicObject
    {
        [XmlIgnore]
        private esEntity _entity;

        public esDynamic() { }

        public esDynamic(esEntity entity) 
        {
            _entity = entity;
        }

        #region DynamicObject Stuff

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            Dictionary<string, object> extra = _entity.GetExtraColumns();
            return extra.Keys;
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return base.GetMetaObject(parameter);
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            return base.TryBinaryOperation(binder, arg, out result);
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            return base.TryConvert(binder, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result)
        {
            return base.TryCreateInstance(binder, args, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
        {
            return base.TryDeleteIndex(binder, indexes);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            return base.TryDeleteMember(binder);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            return base.TryGetIndex(binder, indexes, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            bool found = _entity.currentValues.TryGetValue(binder.Name, out result);
            if (result is System.DBNull) result = null;
            return found;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return base.TryInvokeMember(binder, args, out result);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            return base.TrySetIndex(binder, indexes, value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _entity.currentValues[binder.Name] = value;
            return true;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            return base.TryUnaryOperation(binder, out result);
        }

        #endregion
    }
}
