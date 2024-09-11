﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.FilmInternalUtilities.Editor {

internal static class ExposedReferenceEditorUtility {
    internal static void RecreateReferencesInObject(object obj, IExposedPropertyTable propertyTable) {
        if (null == obj)
            return;
        RecreateReferencesInObject(obj, obj.GetType(), propertyTable);
    }

    internal static void RecreateReferencesInObject(object obj, Type t, IExposedPropertyTable propertyTable) {
        const BindingFlags FIND_EXPOSED_REF_BINDING_FLAGS = BindingFlags.Public
            | BindingFlags.Instance
            | BindingFlags.NonPublic;

        t.GetFields(FIND_EXPOSED_REF_BINDING_FLAGS).Loop((FieldInfo fieldInfo) => {
            //Preliminary check 
            Type fieldType = fieldInfo.FieldType;
            if (fieldType.IsPrimitive || fieldType.IsEnum)
                return;
            if (fieldType == typeof(string))
                return;

            //Call recursively if not generic  
            if (!fieldType.IsGenericType) {
                if (fieldType.IsArray) {
                    RecreateReferencesInList((System.Collections.IList)fieldInfo.GetValue(obj), propertyTable);
                    return;
                }

                object child = fieldInfo.GetValue(obj);
                if (null != child) RecreateReferencesInObject(child, fieldType, propertyTable);
                return;
            }

            if (fieldType.GetGenericTypeDefinition() == typeof(List<>)) {
                RecreateReferencesInList((System.Collections.IList)fieldInfo.GetValue(obj), propertyTable);
                return;
            }

            if (fieldType.GetGenericTypeDefinition() != typeof(ExposedReference<>))
                return;

            RecreateReference(ref obj, fieldInfo, propertyTable);
        });
    }

    private static void RecreateReferencesInList(System.Collections.IList list, IExposedPropertyTable propertyTable) {
        if (null == list)
            return;

        int numObjects = list.Count;
        for (int i = 0; i < numObjects; ++i) {
            RecreateReferencesInObject(list[i], propertyTable);
        }
    }

    internal static void RecreateReference(ref object obj, FieldInfo fieldInfo, IExposedPropertyTable table) {
        Assert.IsNotNull(obj);
        Assert.IsNotNull(fieldInfo);
        Assert.IsNotNull(table);

        object exposedRefObject = fieldInfo.GetValue(obj);
        if (null == exposedRefObject) return;

        //Get the current property name
        FieldInfo    exposedNameField = exposedRefObject.GetType().GetField(EXPOSED_NAME_FIELD_NAME);
        PropertyName curPropertyName  = (PropertyName)exposedNameField.GetValue(exposedRefObject);

        if (null == curPropertyName) return;

        UnityEngine.Object resolvedObject = table.Resolve(curPropertyName);
        if (null == resolvedObject) {
            fieldInfo.SetValue(obj, null); //reset
            return;
        }

        PropertyName newGUID = new PropertyName(GUID.Generate().ToString());

        // [Note-sin: 2022-6-3] What we are trying to do:
        // fieldInfo.SetValue(obj, new ExposedReference<T>() {
        //     exposedName = newGUID
        // });
        object duplicatedRefObject = Activator.CreateInstance(exposedRefObject.GetType());
        exposedNameField.SetValue(duplicatedRefObject, newGUID);
        fieldInfo.SetValue(obj, duplicatedRefObject);

        table.SetReferenceValue(newGUID, resolvedObject);
    }

//----------------------------------------------------------------------------------------------------------------------    

    internal const string EXPOSED_NAME_FIELD_NAME = "exposedName";

}

} //end namespace