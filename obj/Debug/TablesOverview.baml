   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   I ERestaurant POS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:FlamyPOSFlamyPOS 5   	      FlamyPOS.TablesOverview   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   (localclr-namespace:FlamyPOS     5   	   -   5          � Title$  	Flamy POS��5	   	    � WindowState$ 	Maximized=�6    � WindowStyle$
 None=�63    � Triggers 5
      :� 5   
    :� RoutedEvent$ Window.Loaded��6   .[�5      �� .��5      �� .��5      W�  g� Duration$ 0:0:0.2D�6&    �� TargetProperty$ Opacity�69    W� From$ 0X�6]   	 W� To$	 1.0X�6f   5      5      5      	 � 	Resources	 5         TableButton
 �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   ShadowDepth �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName �� Value �� Binding �� Path �� RelativeSource��%�  &	        �� 5   
   "
 ��6$   .�5      ��  � ��$ 20(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��
 �����6+   �� 5       �@J�Effect 5        5      $ 5Q�6'   $ 8Q�5   '   5      �� 5       � �� 5       �� 5!      " ��6&   .R�5"      �� $ border��6!   
������$��3q�5$   !   ���#444242#��z��5&   !   #��z��5'   !   ._�5(      �� $��Center=�6/   $��Center=�5)   /   $��2q�5*   /   5+       5,      P� 5-       �@J�IsMouseOver . . �51   "   �� $ border��6)    � ��
 �   �53   )   54      P� 55       � �� . . �56   "   �� $ border��6)    �@J�Opacity$	 0.7Q�58   )   59      �� 5:      $
 True��5;   *    5:   *   �� $	 Tag� �� *Self+.a�5<   "   �� $ border��6)    � �� �#6666ff6S   5=      5?      5A      5B      5D      .��5E      �  � RowDefinitions 5F   
   �� 5G      $
��Auto �6   �� 5H      $
��Auto �6   �� 5I      $��* �6   �� 5J      $
��Auto �6   .+�5M   
   �� ���#5a5782
�������$��20(�5P      $��35��6   $��Center=�5Q      $��Center=�62   ��5M      �� *Time+5Q   2   ]� 5S   
   $��1��6   ��.  
����p��.+�5W      ��   transparentButtonPurple#��[ 5X      $
��Left=�5Y      $	��140��5Z      ��5W      �� *UserProfileCommand+.��5[      ��  �� Orientation$ 
Horizontal=�6   .+�5\      �� $��Images\user-account.png��6   $��25��6=   $��25��6K   �� 5]      
�������$��20(�5_       �� ContentStringFormat$ Logged in as {0}��5`      ��5]      �� *CurrentUser.Name+5`      5a      5b      �� 5d      $��Right=�5e      
�������$��20(�5g      ��5d      �� *Date+5g      5h      � 5j   
   $��2��6   .+�5k      �� $��2��6   ���#bfbed3���#bfbed3
�������$��2q�6g   $��5q�6{   $��10q�6�   ��5l      �� 5m       �� CornerRadius$ 10��6   ���#5a5782 �� Padding$ 5q�6D   ._�5n      �� $��Table Overview��6$   $��25(�6:   
�������6H   5o      .��5q      �� -   5         ��Name$ TableLayout��5q      $
��1494��6.   $��647.746��6;   5s      5t      5u      A� 5w   
   $��3��6    A� Columns$ 3��5x      ���#5a5782$��5q�5z      $��50��5{      .+�5|      ��   blackThemeBtn#��[ 5}      $	��500��5~      ��5|      �� *AdminOptionsCommand+{�5      �� *	IsAdmin+.��5�      Admin Options6%   �� 5�      #��[ 5�      $	��500��5�      ��5�      �� *TableEditorOptionsCommand+{�5�      �� *	IsAdmin+.��63   Table Editor6A   �� 5�      #��[ 5�      $	��500��5�      ��5�      �� *LogOutCommand+.��5�      	Log Out6   5�      5�      5�      