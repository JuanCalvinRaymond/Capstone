/*UPDATED AS OF: THURSDAY, FEBRUARY 2, 2017*/

using System;

using System.Reflection;

/*
Description: Class used to ensure that structs and other custom types can be read from  different
projects even if they have a different assembly.
Creator: Alvaro Chavez Mixco
Creation Date:  Tuesday, January 17, 2017
*/
public class CAssemblySerializationBinder : System.Runtime.Serialization.SerializationBinder
{
    /*
    Description: Gets the object type with the desired name, in the executing version
    of this assembly.
    Creator: Alvaro Chavez Mixco
    Creation Date:  Tuesday, January 17, 2017
    */
    public override Type BindToType(string aAssemblyName, string aTypeName)
    {
        //Create the variable for the type we want to deserialize
        Type deserializeType = null;

        //Get the name of the assembly that is currently being executed
        string assemblyCurrentlyExecuting = Assembly.GetExecutingAssembly().FullName;

        // Get the type that will be serialized/deserialized using the passed typename, in the currently executing assembly
        deserializeType = Type.GetType(string.Format("{0}, {1}", aTypeName, assemblyCurrentlyExecuting));

        //Return the requested type, as it is in this assembly
        return deserializeType;
    }
}
