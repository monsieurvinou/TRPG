using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    Repeater horizontalRepeater = new Repeater("Horizontal");
    Repeater verticalRepeater = new Repeater("Vertical");

    public static event EventHandler<InfoEventArgs<Point>> MoveEvent;
    public static event EventHandler<InfoEventArgs<string>> FireEvent;

    protected string[] buttons = new string[] { "Submit", "Cancel", "Fire1" };

    /// <summary>
    /// Unity Update method called every frame
    /// </summary>
	void Update ()
    {
        // Movement events
        int x = this.horizontalRepeater.Update();
        int y = this.verticalRepeater.Update();

        if (x != 0 || y != 0)
        {
            if (MoveEvent != null)
            {
                MoveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
            }
        }

        // Fire events
        for (int i = 0; i < this.buttons.Length; ++i)
        {
            if (Input.GetButtonUp(this.buttons[i]))
            {
                if (FireEvent != null)
                {
                    FireEvent(this, new InfoEventArgs<string>(this.buttons[i]));
                }
            }
        }
    }

    /// <summary>
    /// Repeater class, only used for the axis
    /// </summary>
    private class Repeater
    {
        const float threshold = 0.5f;
        const float rate = 0.25f;

        private float next;
        private bool isHolding;
        private string axis;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="axisName">Axis that need repeating</param>
        public Repeater(string axisName)
        {
            this.axis = axisName;
        }

        /// <summary>
        /// Update of the repeater (have to be called manually)
        /// </summary>
        /// <returns>If the user has to hold, 0. Else, if he holded long enough, -1 or 1</returns>
        public int Update()
        {
            int retValue = 0;
            int value = Mathf.RoundToInt(Input.GetAxisRaw(this.axis));

            if (value != 0)
            {
                if (Time.time > this.next)
                {
                    retValue = value;
                    this.next = Time.time + (this.isHolding ? Repeater.rate : Repeater.threshold);
                    this.isHolding = true;
                }
            }
            else
            {
                this.isHolding = false;
                this.next = 0;
            }

            return retValue;
        }
    }
}
