using Aparte.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Aparte.Common.Sp
{
    [DataContract(IsReference = true)]
    public abstract class BaseProperties
    {
        long? PKTemporary = -new Random().Next();
        public long? FileFolder
        {
            get { return PK ?? this.PKTemporary; }
        }

        public long? TempFolder
        {
            get { return PKTemporary; }
        }

        [DataMember]
        [Key, Display(Order = 0, Name = "PK")]
        public virtual long? PK { get; set; }

        public virtual void Update(BaseProperties newObject)
        {
            Update(newObject, "");
        }

        public virtual void Update(BaseProperties newObject, string propertiesToIngnore)
        {
            if (this.PK != newObject.PK) return;
            var defaultIgnore = "PK, Inserted, LastModified, TimesStamp";
            if (string.IsNullOrEmpty(propertiesToIngnore.Trim()))
                propertiesToIngnore = "";

            var ignoreList = (defaultIgnore + propertiesToIngnore).Split(',');
            var existingProperties = this.GetType().GetProperties();
            var newProperties = newObject.GetType().GetProperties();
            foreach (var existingProp in existingProperties)
            {
                if (ignoreList.Contains(existingProp.Name))
                    continue;

                if (existingProp.CanWrite)
                {
                    var t = existingProp.PropertyType;
                    if (t.IsSubclassOf(typeof(Base)) || t.IsInterface)
                        continue;

                    PropertyInfo newProp = null;
                    foreach (var p in newProperties)
                    {
                        if (p.Name == existingProp.Name)
                        {
                            newProp = p;
                            break;
                        }
                    }
                    existingProp.SetValue(this, newProp.GetValue(newObject, null), null);
                }
            }
        }
    }
}
