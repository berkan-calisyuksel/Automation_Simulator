using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_AutomationSimulator
{
    public enum State
    {
        Start,
        Slow,
        Position,
        Stop
    }

    public class Controller
    {
        public static State CurrentState = State.Start;
        public static double MiddleTriggerTime, RightTriggerTime;

        public static Outputs Update(Inputs inputs)
        {
            Outputs outputs = new Outputs();

            double positionTime = (RightTriggerTime - MiddleTriggerTime) / 4;


            if (CurrentState == State.Start && inputs.ProximitySensorMiddle)
            {
                CurrentState = State.Slow;
                MiddleTriggerTime = inputs.CurrentTimeInMilliseconds;
            }

            else if (CurrentState == State.Slow && inputs.ProximitySensorRight)
            {
                CurrentState = State.Position;
                RightTriggerTime = inputs.CurrentTimeInMilliseconds;
            }

            else if (CurrentState == State.Position && (inputs.CurrentTimeInMilliseconds > (RightTriggerTime + positionTime)))
            {
                CurrentState = State.Stop;
            }

            else if (inputs.ProximitySensorLeft)
            {
                CurrentState = State.Stop;
            }


            if (inputs.PositioningEnabled)
            {
                switch (CurrentState)
                {
                    case State.Start:
                        outputs.MoveRight = true;
                        outputs.MoveSpeed = Configuration.MotorSpeedFast;
                        break;

                    case State.Slow:
                        outputs.MoveRight = true;
                        outputs.MoveSpeed = Configuration.MotorSpeedSlow;
                        break;

                    case State.Position:
                        outputs.MoveLeft = true;
                        outputs.MoveSpeed = Configuration.MotorSpeedSlow;
                        break;
                    case State.Stop:
                        outputs.MoveSpeed = 0;
                        break;
                }
            }

            return outputs;
        }
    }
}