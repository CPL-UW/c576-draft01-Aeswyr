using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float dir {
        get;
        private set;
    }
    public ButtonState jump {
        get{return m_jump;}
    }
    public ButtonState mode {
        get {return m_mode;}
    }
    public ButtonState move {
        get {return m_move;}
    }
    public ButtonState interact {
        get {return m_interact;}
    }
    private ButtonState m_mode, m_jump, m_move, m_interact;

    private void FixedUpdate() {
        this.m_jump.Reset();
        this.m_mode.Reset();
        this.m_move.Reset();
        this.m_interact.Reset();
    }

    public void Move(InputAction.CallbackContext ctx) {
        this.dir = ctx.ReadValue<float>();
        this.m_move.Set(ctx);
    }

    public void Jump(InputAction.CallbackContext ctx) {
        this.m_jump.Set(ctx);
    }

    public void Mode(InputAction.CallbackContext ctx) {
        this.m_mode.Set(ctx);
    }

    public void Interact(InputAction.CallbackContext ctx) {
        this.m_interact.Set(ctx);
    }

    public struct ButtonState {
        private bool firstFrame;
        public bool down {
            get;
            private set;
        }
        public bool pressed {
            get {
                return down && firstFrame;
            }
        }
        public bool released {
            get {
                return !down && firstFrame;
            }
        }

        public void Set(InputAction.CallbackContext ctx) {
            down = !ctx.canceled;             
            firstFrame = true;
        }
        public void Reset() {
            firstFrame = false;
        }
    }
}
