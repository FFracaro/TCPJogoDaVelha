using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Validations : MonoBehaviour
{
    public bool ValidateIpAndPort(string ip, string port)
    {
        UIManager_InitialScene UIManagerInitial = GetComponent<UIManager_InitialScene>();
        if (ValidadeIpAddress(ip, UIManagerInitial))
        {
            if (ValidadePort(port, UIManagerInitial))
            {              
                return true;
            }
        }
        return false;
    }

    private bool ValidadeIpAddress(string ip, UIManager_InitialScene UI)
    {
        string regexPattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
        Regex IpChecker = new Regex(regexPattern);

        if(string.IsNullOrEmpty(ip))
        {
            // string ip nula ou vazia
            UI.ShowErrorPopUpMessage("Campo IP nulo ou vazio.");
            return false;
        }

        if (!IpChecker.IsMatch(ip, 0))
        {
            UI.ShowErrorPopUpMessage("Campo IP não válido.");
            // ip não válido
            return false;
        }

        return true;
    }

    private bool ValidadePort(string port, UIManager_InitialScene UI)
    {
        if (string.IsNullOrEmpty(port))
        {
            UI.ShowErrorPopUpMessage("Campo porta nulo ou vazio.");
            // string port nula ou vazia
            return false;
        }

        if (!DigitsOnly(port))
        {
            UI.ShowErrorPopUpMessage("Porta contém dígitos não-númericos.");
            // port contains non numeric digits
            return false;
        }

        // 0 to 65535 ports
        int portInt = int.Parse(port);

        if (!(int.Parse(port) >= 0 && portInt <= 65535))
        {
            UI.ShowErrorPopUpMessage("Porta fora do intervalo 0-65.535.");
            // port out of range
            return false;
        }

        return true;
    }

    private bool DigitsOnly(string s)
    {
        foreach (char c in s)
        {
            if (c < '0' || c > '9')
                return false;
        }
        return true;
    }

}
