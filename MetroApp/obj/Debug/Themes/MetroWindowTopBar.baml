   M S B A M L   `   `   `  ����C  ?MetroApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:MetroApp.ControlsMetroApp.Controls  5      1clr-namespace:MetroApp.ThemesMetroApp.Themes  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089)    #MetroApp.Controls.MetroWindowTopBar  �� 
TargetType   MetroWindowTopBarStyle �� Property �� Value   MetroApp.Themes.BrushKeys   WindowForegroundBrushKey �� 
TargetType � ColumnDefinitions ��Name �� RelativeSource �� AncestorType#   MetroApp.Controls.MetroWindow	 �� Path
 ��Name    RightWindowCommands/   )MetroApp.Controls.MetroWindowStateButtons  Name �� BasedOn�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      ,Controlsclr-namespace:MetroApp.Controls  5      (Themesclr-namespace:MetroApp.Themes  5      %   &	        '   �    7   �� 5      "    6   .�5   
   ��  � ��
 ���� 6'   �� 5   
    � ��# �  6'   �� 5	   
    � ��$ 25��6#   �� 5
   
    � �� 5      �� 5      "   6"   .R�5      � #��z��6    5      �� 5      $
��Auto �6/   �� 5      $
��Auto �6/   �� 5      $��* �6/   �� 5      $
��Auto �6/   �� 5      $
��Auto �6/   .+�5      �� $ 	PART_Icon��6)   |�.  $��0��5   1   $��Stretch=�5!   1   $��Stretch=�5"   1   ��5   1   ��  �� *FindAncestor+"  $
	 Icon���5    1   �� *IconTemplate+5"   1   �� 5$      $
 PART_LeftWindowCommands��6+   |�.  $��1��5'   3   ��5&   3   �� *LeftWindowCommands+5'   3   �� 5*      $ PART_TitleBar��6)   |�.  $��2��5,   1   $��Stretch=�5.   1   $��Stretch=�5/   1   ��5-   1   �� *TitleTemplate+��50      �� 51   "   $��10,0,0,0q�6(   #��z��52   (   #��z��53   (   ��51   :   ��  �� *FindAncestor+"  $	 Title�53   (   55      �� 57      $
 PART_RightWindowCommands��6+   $��3��58   +   |�.  #��z 5:   +     5=      $ PART_WindowButtonCommands��6;   |�.  $��4��5?   D   5@      5A      5C      5D      �� 5F      "    6   8   6>   5H      