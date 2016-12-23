using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class StringExtension{

    public static float ToFloat(this string args)
    {
        float output;
        try
        {
            output = float.Parse(args); 
        }
        catch (FormatException e)
        {
            output = 0.0f;
            DebugManager.mInstance.OutputMsg(e, ELogCategory.Default, true);
        }
        return output;  
    }
	
}
