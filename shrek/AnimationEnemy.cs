using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shrek
{
    class AnimationEnemy
    {
        public enum states { enemy_run, enemy_dead }
        Dictionary<states, List<Image>> animationStates = new Dictionary<states, List<Image>>();
        int lastIndex;
        int interval = 200;
        DateTime lastGet = DateTime.Now;
        public states CurrentState = states.enemy_run;
        public AnimationEnemy(states state, int interval)
        {
            this.interval = interval;
            animationStates.Add(state, new List<Image>());
            string[] files = Directory.GetFiles("images", state.ToString() + "?.png");
            for (int i = 0; i < files.Length; i++)
                animationStates[state].Add(Image.FromFile(files[i]));
        }

        public void LoadAnimationForState(states stateName)
        {
            animationStates.Add(stateName, new List<Image>());
            string[] files = Directory.GetFiles("images", stateName.ToString() + "?.png");
            for (int i = 0; i < files.Length; i++)
                animationStates[stateName].Add(Image.FromFile(files[i]));
        }

        public Image getImage()
        {
            if ((DateTime.Now - lastGet).Milliseconds > interval)
            {
                lastGet = DateTime.Now;
                if (++lastIndex < animationStates[CurrentState].Count)
                {
                    return animationStates[CurrentState][lastIndex];
                }
                else
                {
                    lastIndex = 0;
                    return animationStates[CurrentState][lastIndex];
                }
            }
            else
            {
                if (lastIndex > animationStates.Count)
                {
                    lastIndex = 1;
                }
                return animationStates[CurrentState][lastIndex];
            }
        }

        public void ChangeState(states state)
        {
            CurrentState = state;
        }
    }
}
