   M S B A M L   `   `   `  ����I  ERestaurant POS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:FlamyPOSFlamyPOS  5   	       FlamyPOS.ChangeDueWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:FlamyPOS     5   	     � Title$  ChangeDueWindow��5   	   $	��475��6!   $	��500��6.    � WindowStartupLocation$ CenterScreen=�5	   	    � WindowStyle$
 None=�5
   	    � AllowsTransparency .  � 
ResizeMode$ CanResizeWithGrip=�66    � Triggers 5      :� 5   
    :� RoutedEvent$ Window.Loaded��6   .[�5      �� .��5      �� .��5      W�  g� Duration$ 0:0:.5D�6&    �� TargetProperty$	 Top�68   	 W� From$	 0X�6X   	
 W� To$
 237.5X�6a    g� AccelerationRatio$ .3Q�6l   5      5      5       � 	Resources 5         orangeBrush �� 
StartPoint �� EndPoint � Color � Offset  orangeButton �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property P� Value3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   ShadowDepth   Color��%t  &	        &	 '    7   7   �� 5   
   $ 0.5,0&�62   $ 0.5,1&�6E   .��5      � $ #ffcc80��6   $ 0Q�6+   � 5      $ #ffc947��6   $	 0.2Q�6+   � 5      $ #ff9800��6   $	 0.5Q�6+   � 5      $ Orange��6   $	 0.8Q�6*   � 5      $ #e65100��6   $ 1Q�6+   5      �� 5   
   " ��6%   .�5       ��  � ��
 �����6+   �� 5!       � ��$ Medium"�6+   �� 5"       � ��$ Fonts\Billabong.ttf)�6+   �� 5#       � �� 5$      �� 5%      " ��6&   .R�5&      �� $ 30��5'      8��  8�� $��2q�5*      ._�5+      �� #��z��6/   $��Center=�5,   /   $��Center=�5-   /   5.       5/      P� 50       � �� . . �51   "   ��  �@J�Effect 52   &     53   *   $ 20Q�6;   $ 10Q�54   ;   $ Orange��55   ;   57   #   58      5:      5<      5=      5>      .��5@      �� 
���   �$��1q�6!   
�������._�5A   
   �  � RowDefinitions 5B      �� 5C      $
��Auto �6    �� 5D      $
��Auto �6    �� 5E      $��* �6    �� 5F      $
��Auto �6    �� 5G      $
��Auto �6    .+�5I      �� $��0��6   $��15��6!   ���#c671006-   �� 5J      $��1��6   ���#ffa000
�������$��26(�6I   $��Center=�6X   .��6|   
Change Due6�   �� 5K      $��2��6   $��Images\currency.png��6!   $	��100��5L      $	��250��5M      �� 5O      $��3��6   
�������$��2q�5Q      $��
75 0 75 20q�5R      ._�5S      �� $��3��6    �� ContentStringFormat$ 	RM {0:F2}��5U      
�������$��26(�5W      $	��100��5X      $��Medium"�5Y      $��Center=�5Z      $��Center=�5[      ��5T      �� *	ChangeDue+5\      5]      �� 5_      $��4��6   $��80��5a      $��32(�5b      $��150 0 150 20q�5c      #��[ 5d      ��5`      �� *ReturnCommand+.��5d   ;   OK6?   5e      5f      5g      