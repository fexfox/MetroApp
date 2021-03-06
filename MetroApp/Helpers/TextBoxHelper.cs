﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace MetroApp.Helpers
{
    /// <summary>
    /// A helper class that provides various attached properties for the TextBox control.
    /// </summary>
    /// <remarks>
    /// Password watermarking code from: http://prabu-guru.blogspot.com/2010/06/how-to-add-watermark-text-to-textbox.html
    /// </remarks>
    public class TextBoxHelper
    {
        public static readonly DependencyProperty IsMonitoringProperty = DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(TextBoxHelper), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(TextBoxHelper), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty TextLengthProperty = DependencyProperty.RegisterAttached("TextLength", typeof(int), typeof(TextBoxHelper), new UIPropertyMetadata(0));
        public static readonly DependencyProperty ClearTextButtonProperty = DependencyProperty.RegisterAttached("ClearTextButton", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false, ButtonCommandOrClearTextChanged));

        public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.RegisterAttached("ButtonCommand", typeof(ICommand), typeof(TextBoxHelper), new FrameworkPropertyMetadata(null, ButtonCommandOrClearTextChanged));
        public static readonly DependencyProperty ButtonCommandParameterProperty = DependencyProperty.RegisterAttached("ButtonCommandParameter", typeof(object), typeof(TextBoxHelper), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.RegisterAttached("ButtonContent", typeof(object), typeof(TextBoxHelper), new FrameworkPropertyMetadata("r"));
        public static readonly DependencyProperty ButtonTemplateProperty = DependencyProperty.RegisterAttached("ButtonTemplate", typeof(ControlTemplate), typeof(TextBoxHelper), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ButtonFontFamilyProperty = DependencyProperty.RegisterAttached("ButtonFontFamily", typeof(FontFamily), typeof(TextBoxHelper), new FrameworkPropertyMetadata((new FontFamilyConverter()).ConvertFromString("Marlett")));

        public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsWaitingForDataProperty = DependencyProperty.RegisterAttached("IsWaitingForData", typeof(bool), typeof(TextBoxHelper), new UIPropertyMetadata(false));

        public static readonly DependencyProperty FocusBorderBrushProperty = DependencyProperty.RegisterAttached("FocusBorderBrush", typeof(Brush), typeof(TextBoxHelper), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof(Brush), typeof(TextBoxHelper), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        private static readonly DependencyProperty HasTextProperty = DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata(false));

        // Gets a fresh context menu. 
        private static ContextMenu GetDefaultTextBoxBaseContextMenu()
        {
            var defaultMenu = new ContextMenu();

            var m1 = new MenuItem { Command = ApplicationCommands.Cut };
            m1.SetResourceReference(FrameworkElement.StyleProperty, "MetroMenuItem");
            var m2 = new MenuItem { Command = ApplicationCommands.Copy };
            m2.SetResourceReference(FrameworkElement.StyleProperty, "MetroMenuItem");
            var m3 = new MenuItem { Command = ApplicationCommands.Paste };
            m3.SetResourceReference(FrameworkElement.StyleProperty, "MetroMenuItem");

            defaultMenu.Items.Add(m1);
            defaultMenu.Items.Add(m2);
            defaultMenu.Items.Add(m3);

            return defaultMenu;
        }

        /// <summary>
        /// Sets the brush used to draw the focus border.
        /// </summary>
        public static void SetFocusBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(FocusBorderBrushProperty, value);
        }

        /// <summary>
        /// Gets the brush used to draw the focus border.
        /// </summary>
        public static Brush GetFocusBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FocusBorderBrushProperty);
        }

        /// <summary>
        /// Sets the brush used to draw the mouse over brush.
        /// </summary>
        public static void SetMouseOverBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverBorderBrushProperty, value);
        }

        /// <summary>
        /// Gets the brush used to draw the mouse over brush.
        /// </summary>
        public static Brush GetMouseOverBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBorderBrushProperty);
        }

        public static void SetIsWaitingForData(DependencyObject obj, bool value)
        {
            obj.SetValue(IsWaitingForDataProperty, value);
        }

        public static bool GetIsWaitingForData(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsWaitingForDataProperty);
        }

        public static void SetSelectAllOnFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectAllOnFocusProperty, value);
        }

        public static bool GetSelectAllOnFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectAllOnFocusProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }

        private static void SetTextLength(DependencyObject obj, int value)
        {
            obj.SetValue(TextLengthProperty, value);
            obj.SetValue(HasTextProperty, value >= 1);
        }

        /// <summary>
        /// Gets if the attached TextBox has text.
        /// </summary>
        public static bool GetHasText(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasTextProperty);
        }

        public static void SetHasText(DependencyObject obj, bool value)
        {
            obj.SetValue(HasTextProperty, value);
        }

        static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox)
            {
                var txtBox = d as TextBox;

                if ((bool)e.NewValue)
                {
                    txtBox.TextChanged += TextChanged;
                    txtBox.GotFocus += TextBoxGotFocus;
                }
                else
                {
                    txtBox.TextChanged -= TextChanged;
                    txtBox.GotFocus -= TextBoxGotFocus;
                }
            }
            else if (d is PasswordBox)
            {
                var passBox = d as PasswordBox;

                if ((bool)e.NewValue)
                {
                    passBox.PasswordChanged += PasswordChanged;
                    passBox.GotFocus += PasswordGotFocus;

                    // issue 1343: the watermark exists if the password was set in xaml (binding etc)
                    var pw = passBox.Password;
                    passBox.Clear();
                    passBox.Password = pw;
                }
                else
                {
                    passBox.PasswordChanged -= PasswordChanged;
                    passBox.GotFocus -= PasswordGotFocus;
                }
            }
        }

        static void TextChanged(object sender, TextChangedEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox == null)
                return;
            SetTextLength(txtBox, txtBox.Text.Length);
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passBox = sender as PasswordBox;
            if (passBox == null)
                return;
            SetTextLength(passBox, passBox.Password.Length);
        }

        static void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox == null)
                return;
            if (GetSelectAllOnFocus(txtBox))
            {
                txtBox.Dispatcher.BeginInvoke((Action)(txtBox.SelectAll));
            }
        }

        static void PasswordGotFocus(object sender, RoutedEventArgs e)
        {
            var passBox = sender as PasswordBox;
            if (passBox == null)
                return;
            if (GetSelectAllOnFocus(passBox))
            {
                passBox.Dispatcher.BeginInvoke((Action)(passBox.SelectAll));
            }
        }

        public static bool GetClearTextButton(DependencyObject d)
        {
            return (bool)d.GetValue(ClearTextButtonProperty);
        }

        public static void SetClearTextButton(DependencyObject obj, bool value)
        {
            obj.SetValue(ClearTextButtonProperty, value);
        }

        public static ICommand GetButtonCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(ButtonCommandProperty);
        }

        public static void SetButtonCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ButtonCommandProperty, value);
        }

        public static object GetButtonCommandParameter(DependencyObject d)
        {
            return (object)d.GetValue(ButtonCommandParameterProperty);
        }

        public static void SetButtonCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(ButtonCommandParameterProperty, value);
        }

        public static object GetButtonContent(DependencyObject d)
        {
            return (object)d.GetValue(ButtonContentProperty);
        }

        public static void SetButtonContent(DependencyObject obj, object value)
        {
            obj.SetValue(ButtonContentProperty, value);
        }

        public static ControlTemplate GetButtonTemplate(DependencyObject d)
        {
            return (ControlTemplate)d.GetValue(ButtonTemplateProperty);
        }

        public static void SetButtonTemplate(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(ButtonTemplateProperty, value);
        }

        public static FontFamily GetButtonFontFamily(DependencyObject d)
        {
            return (FontFamily)d.GetValue(ButtonFontFamilyProperty);
        }

        public static void SetButtonFontFamily(DependencyObject obj, FontFamily value)
        {
            obj.SetValue(ButtonFontFamilyProperty, value);
        }

        private static void ButtonCommandOrClearTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = d as TextBox;
            if (textbox != null)
            {
                // only one loaded event
                textbox.Loaded -= TextBoxLoaded;
                textbox.Loaded += TextBoxLoaded;
            }
            var passbox = d as PasswordBox;
            if (passbox != null)
            {
                // only one loaded event
                passbox.Loaded -= PassBoxLoaded;
                passbox.Loaded += PassBoxLoaded;
            }
            var combobox = d as ComboBox;
            if (combobox != null)
            {
                // only one loaded event
                combobox.Loaded -= ComboBoxLoaded;
                combobox.Loaded += ComboBoxLoaded;
            }
        }

        static void ComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null || comboBox.Style == null)
                return;

            var template = comboBox.Template;
            if (template == null)
                return;

            var dropDown = template.FindName("PART_DropDownToggle", comboBox) as ToggleButton;
            if (dropDown == null || dropDown.Template == null)
                return;

            var button = dropDown.Template.FindName("PART_ClearText", dropDown) as Button;
            if (button == null)
                return;

            if (GetClearTextButton(comboBox))
            {
                // only one event, because loaded event fires more than once, if the textbox is hosted in a tab item
                button.Click -= ButtonClicked;
                button.Click += ButtonClicked;
                comboBox.SetValue(HasTextProperty, !string.IsNullOrWhiteSpace(comboBox.Text) || comboBox.SelectedItem != null);
            }
            else
            {
                button.Click -= ButtonClicked;
            }
        }

        static void PassBoxLoaded(object sender, RoutedEventArgs e)
        {
            var passbox = sender as PasswordBox;
            if (passbox == null || passbox.Style == null)
                return;

            var template = passbox.Template;
            if (template == null)
                return;

            var button = template.FindName("PART_ClearText", passbox) as Button;
            if (button == null)
                return;

            if (GetButtonCommand(passbox) != null || GetClearTextButton(passbox))
            {
                // only one event, because loaded event fires more than once, if the textbox is hosted in a tab item
                button.Click -= ButtonClicked;
                button.Click += ButtonClicked;
                passbox.SetValue(HasTextProperty, !string.IsNullOrWhiteSpace(passbox.Password));
            }
            else
            {
                button.Click -= ButtonClicked;
            }
        }

        static void TextBoxLoaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null || textbox.Style == null)
                return;

            var template = textbox.Template;
            if (template == null)
                return;

            var button = template.FindName("PART_ClearText", textbox) as Button;
            if (button == null)
                return;

            if (GetButtonCommand(textbox) != null || GetClearTextButton(textbox))
            {
                // only one event, because loaded event fires more than once, if the textbox is hosted in a tab item
                button.Click -= ButtonClicked;
                button.Click += ButtonClicked;
                textbox.SetValue(HasTextProperty, !string.IsNullOrWhiteSpace(textbox.Text));
            }
            else
            {
                button.Click -= ButtonClicked;
                textbox.SetValue(HasTextProperty, !string.IsNullOrWhiteSpace(textbox.Text));
            }
        }

        static void ButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = ((Button)sender);
            var parent = VisualTreeHelper.GetParent(button);
            while (!(parent is TextBox || parent is PasswordBox || parent is ComboBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            var command = GetButtonCommand(parent);
            if (command != null && command.CanExecute(parent))
            {
                var commandParameter = GetButtonCommandParameter(parent);

                command.Execute(commandParameter ?? parent);
            }

            if (GetClearTextButton(parent))
            {
                if (parent is TextBox)
                {
                    ((TextBox)parent).Clear();
                }
                else if (parent is PasswordBox)
                {
                    ((PasswordBox)parent).Clear();
                }
                else if (parent is ComboBox)
                {
                    if (((ComboBox)parent).IsEditable)
                    {
                        ((ComboBox)parent).Text = string.Empty;
                    }
                    ((ComboBox)parent).SelectedItem = null;
                }
            }
        }
    }
}
