   M S B A M L   `   `   `  ����I  ERestaurant POS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:FlamyPOSFlamyPOS  5   	       FlamyPOS.TableChange   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:FlamyPOS     5   	     � Title$  	Flamy POS��5   	    � WindowState$ 	Maximized=�6    � WindowStyle$
 None=�63    � Triggers 5	      :� 5
   
    :� RoutedEvent$ Window.Loaded��6   .[�5      �� .��5      �� .��5      W�  g� Duration$ 0:0:0.2D�6&    �� TargetProperty$ Opacity�69    W� From$ 0X�6]   	 W� To$	 1.0X�6f   5      5      5      	 � 	Resources	 5         TableButton
 �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   ShadowDepth �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName �� Value �� Binding �� Path �� RelativeSource��%�  &	        �� 5   
   "
 ��6$   .�5      ��  � ��$ 20(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��
 �����6+   �� 5       �@J�Effect 5        5      $ 5Q�6'   $ 8Q�5   '   5      �� 5       � �� 5      �� 5       " ��6&   .R�5!      �� $ border��6!   
������$��3q�5#   !   ���#444242#��z��5%   !   #��z��5&   !   ._�5'      �� $��Center=�6/   $��Center=�5(   /   $��2q�5)   /   5*       5+      P� 5,       �@J�IsMouseOver . . �50   "   �� $ border��6)    � ��
 �   �52   )   53      P� 54       � �� . . �55   "   �� $ border��6)    �@J�Opacity$	 0.7Q�57   )   58      �� 59      $
 True��5:   *    59   *   �� $	 Tag� �� *Self+.a�5;   "   �� $ border��6)    � �� �#6666ff6S   5<      5>      5@      5A      5B      .��5D      �  � RowDefinitions 5E   
   �� 5F      $
��Auto �6   �� 5G      $
��Auto �6   �� 5H      $��* �6   �� 5I      $
��Auto �6   .+�5L   
   �� ���#5a5782
�������$��20(�5O      $��35��6   $��Center=�5P      $��Center=�62   ��5L      �� *Time+5P   2   � 5R   
   $��1��6   
����p��$��20��5T      5V      � 5X   
   $��2��6   .+�5Y      �� $��2��6   ���#bfbed3���#bfbed3
�������$��2q�6g   $��5q�6{   $��10q�6�   ��5Z      �� 5[       �� CornerRadius$ 10��6   ���#5a5782 �� Padding$ 5q�6D   ._�5\      �� 
�������$��24(�5^        �� ContentStringFormat$&  Previous table chosen: Table {0}��5_       ��5\       �� *PreviousTableName+5_       5`      .��5b      �� -   5         ��Name$ TableLayout��5b      $
��1494��6.   $��647.746��6;   5d      5e      5f      A� 5h   
   $��3��6    A� Columns$ 2��5i      ���#5a5782$��5q�5k      $��50��5l      .+�5m      ��   blackThemeBtn#��[ 5n      $	��500��5o      ��5m      �� *ReturnCommand+.��5p      Return6   5q      5r      5s      