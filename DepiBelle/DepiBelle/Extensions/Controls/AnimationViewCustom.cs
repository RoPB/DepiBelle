﻿using System;
using System.Windows.Input;
using DepiBelle.Models.Lottie;
using Lottie.Forms;
using Xamarin.Forms;

namespace DepiBelle.Extensions
{
    public class AnimationViewCustom : AnimationView
    {
        public static readonly BindableProperty PlayCommandProperty =
            BindableProperty.Create(nameof(PlayCommand), typeof(ICommand), typeof(AnimationViewCustom), null, BindingMode.OneWayToSource);

        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandProperty); }
            set { SetValue(PlayCommandProperty, value); }
        }

        public AnimationViewCustom()
        {
            PlayCommand = new Command((param) => PlayAnimation(param));
        }

        private void PlayAnimation(object param = null)
        {
            var lottieProgress = param as LottieProgress;

            if (!IsPlaying){
                Loop = lottieProgress.Loop;
                this.PlayProgressSegment(lottieProgress.From, lottieProgress.To);//Play();
            }

        }


    }
}
