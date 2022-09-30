
using System.Collections.Generic;
using RougeFW;
using UnityEngine;
using UnityEngine.Events;

using Object = System.Object;


    public class MsgSystem : MonoBehaviour
    {
        
        public static MsgSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }



        public bool is_pause = false;


    /* public Dictionary<string, List<UnityAction<Object[]>>> msg_actions =
         new Dictionary<string, List<UnityAction<Object[]>>>();*/
    public GenericDictionary<string, List<UnityAction<Object[]>>> msg_actions = new GenericDictionary<string, List<UnityAction<Object[]>>>();
    public void RegistMsgAction(string msg_name, UnityAction<Object[]> msg_action)
        {
        /* if (!msg_actions.ContainsKey(msg_name))
         {
             var actionList = new List<UnityAction<Object[]>>();
             actionList.Add(msg_action);
             msg_actions.Add(msg_name, actionList);
         }
         else
         {
             msg_actions[msg_name].Add(msg_action);
         }*/
        UtilitySystem.AddItemToDictionaryWithListValue(msg_actions, msg_name, msg_action);

    }


        public void RemoveMsgAction(string msg_name, UnityAction<Object[]> msg_action)
        {
            msg_actions[msg_name].Remove(msg_action);
        }


        public void SendMsg(string msg_name, Object[] msg_content)
        {
            if (msg_actions.ContainsKey(msg_name) == true)
                for (int i = 0; i < msg_actions[msg_name].Count; i++)
                    msg_actions[msg_name][i](msg_content);
        }



        public void ClearAllMsg()
        {
            msg_actions.Clear();
        }





        public static string vr_trigger_down_left = "vr_trigger_down_left";
        public static string vr_trigger_up_left = "vr_trigger_up_left";
        public static string vr_trigger_down_right = "vr_trigger_down_right";
        public static string vr_trigger_up_right = "vr_trigger_up_right";

        public static string vr_hold_down_left = "vr_hold_down_left";
        public static string vr_hold_up_left = "vr_hold_up_left";
        public static string vr_hold_down_right = "vr_hold_down_right";
        public static string vr_hold_up_right = "vr_hold_up_right";

        public static string vr_button_a_down = "vr_button_a_down";
        public static string vr_button_a_up = "vr_button_a_up";

        public static string vr_button_b_down = "vr_button_b_down";
        public static string vr_button_b_up = "vr_button_b_up";

        public static string vr_button_x_down = "vr_button_x_down";
        public static string vr_button_x_up = "vr_button_x_up";

        public static string vr_button_y_down = "vr_button_y_down";
        public static string vr_button_y_up = "vr_button_y_up";

        public static string battle_ready = "battle_ready";

        public static string left_gun_shot = "left_gun_shot";
        public static string right_gun_shot = "right_gun_shot";

        public static string resources_ready = "resources_ready";
        
        public static string getted_level_message = "getted_level_message";
        public static string getted_all_assets = "getted_all_assets";
        [System.Serializable]
        public class DelayAction
        {
            public string action_id = "";
            public float start_time = 0;
            public float delay_time = 0;
            public UnityAction delay_action;
            public bool is_loop = false;

            public DelayAction(float delay, UnityAction action, string id, bool loop )
            {
                start_time = Time.time;
                delay_time = delay;
                delay_action = action;
                action_id = id;
                is_loop = loop;
            }

            public bool isKilled;
        }

        public List<DelayAction> delay_actions = new List<DelayAction>();


        public void AddDelayAction(float delay, UnityAction action, string action_id, bool is_loop = false )
        {
            delay_actions.Add(new DelayAction(delay, action, action_id,is_loop));
        }


        public void KillDelayAction(string action_id)
        {
            for (int i = delay_actions.Count - 1; i >= 0; i--)
                if (delay_actions[i].action_id.Equals(action_id) == true)
                    delay_actions[i].isKilled = true;
        }


        public void KillAllDelayAction()
        {
            for (int i = delay_actions.Count - 1; i >= 0; i--)
                    delay_actions[i].isKilled = true;
        }

        private void Update()
        {
            if (is_pause == true)
                return;

            for (int i = delay_actions.Count - 1; i >= 0; i--)
                if (delay_actions[i].isKilled)
                {
                    delay_actions.RemoveAt(i);
                }
                else if ((Time.time - delay_actions[i].start_time) > delay_actions[i].delay_time)
                {
                    delay_actions[i].delay_action();

                    if (delay_actions[i].is_loop == false)
                        delay_actions.RemoveAt(i);
                    else
                        delay_actions[i].start_time = Time.time;
                }
        }

    }
