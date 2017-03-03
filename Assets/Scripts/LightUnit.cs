using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LightUnit
{
    // properties
    private int id = 0;
    private bool state = false;
    private float brightness = 0.0f;
    private float color = 0.0f;

    //
    public int ID() { return this.id; }
    public void ID(int value) { this.id = value; }

    public bool State() { return this.state; }
    public void State(bool value) { this.state = value; }
    public void Switch() { this.state = !this.state; }

    public float Brightness() { return this.brightness; }
    public void Brightness(float value) { this.brightness = value; }

    public float Color() { return this.color; }
    public void Color(float value) { this.color = value; }

    // constructors
    public LightUnit(int id) { this.id = id; }
    public LightUnit(int id, bool state, float brightness, float color)
    {
        this.id = id;
        this.state = state;
        this.brightness = brightness;
        this.color = color;
    }

}

