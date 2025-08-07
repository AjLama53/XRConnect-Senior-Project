using System;
using System.Collections.Generic;
using UnityEngine;

public class IoTAuth
{
    private string accessToken;

    public bool Login(string username, string password)
    {
        // Simulated authentication logic  
        if (username == "admin" && password == "password")
        {
            accessToken = "sample_token_123";
            return true;
        }
        return false;
    }

    public string GetAccessToken() => accessToken;
}
