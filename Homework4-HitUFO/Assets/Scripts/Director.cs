using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object {
    private static Director instance;
    public MainController mainController { get; set; }
    public static Director GetInstance() {
        if (instance == null) {
            instance = new Director();
        }
        return instance;
    }
}
