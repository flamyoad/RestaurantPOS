   M S B A M L   `   `   `  ����I  ERestaurant POS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:FlamyPOSFlamyPOS  5   	        FlamyPOS.UserProfileWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:FlamyPOS     5   	     � Title$  UserProfileWindow��5   	    � WindowState$ 	Maximized=�6#    � WindowStyle$
 None=�6;    � 	Resources 5	         PurpleShadeGradient �� 
StartPoint �� EndPoint � Color � Offset  blueTabButton �� 
TargetType	 �� Property
 �� Value �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName ��Name ��Name ]� 
Conditions �� Property �� Value��%�  &	        &	 �     'G� �    �� 5
   
   $	 0,1&�6:   $	 1,0&�6K   .��5      � $ #7B68EE��6   $ 0Q�6+   � 5      $ #6A5ACD��6   $	 0.3Q�6+   � 5      $ #8A2BE2��6   $	 0.6Q�6+   � 5      $ #9400D3��6   $	 0.8Q�6+   5      �� 5   
   " ��6&   .�5      �� 	 � ��

 �   �6+   �� 5      	 � ��$

 Bold"�6+   �� 5      	 � ��$
 Fonts\Robot-Regular.ttf)�6+   �� 5      	 � ��$
 90��6*   �� 5      	 � ��$	
 260��6)   �� 5      	 � ��$
 28(�6)   �� 5      	 � ��$
 0,0,0,0q�6'   �� 5      	 � ��
 5      �� 5      " ��6&   .R�5      �� $ border��6!   $��0,0,0,5q�61   
�������._�5       �� $��Header��5!   !   $��Center=�5"   !   $��Center=�5#   !   #��z��5$   !   5%       5&      P� 5'       �@��
IsSelected . . �5(   "   �� $ border��6)   	 � ��

 ��p��6T   �� 5)   "   $ border��6)   	 � ��

 �����6S   5*      P� 5+       �@��
IsSelected .  . �5,   "   �� $ border��6)   	 � ��

 �����6T   �� 5-   "   $ border��6)   	 � ��

 �����6S   �� 5.   "   	 � ��

 �����6?   5/      51      53      54      �� 57   
   " G�6   .�58      �� 	 � ��$	
 170��6)   �� 59      	 � ��$
 5q�6'   �� 5:      	 � ��
 5;      �� 5<      " G�6&   .R�5=      �� ���#808080$��2q�67   
�������._�5>      � .+�5?   "   �� $ label��6(   $y�Hidden=�5@   (   $��16(�5A   (   
�������$��Center=�5C   (   $
��Left=�5D   (   $��5q�5E   (   .��63   Password (Numeric only)6L   �� 5F   "   $ PART_ContentHost��6/   $��5q�6I   5G      5H       5I      ]� 5J       5K   "   �� 5L   &    �@J�IsKeyboardFocusWithin .  6Q   �� 5M   &    �@�Tag$ False��6?   ./�5O   "   �� $ label��6)   	 � y�$
 Visible=�6R   5P      5R      5T      5U      5V      .��5X      �  � RowDefinitions 5Y   
   �� 5Z      $
��Auto �6   �� 5[      $��* �6   .+�5^   
   ]� $��40��6   ��. ��5`      �� 5a      .��5b      � $ #5a5782��6#   $	 0.2Q�63   � 5c      $ MediumPurple��6#   $	 0.6Q�68   � 5d      $ Purple��6#   $ 1Q�62   5e      .+�5h      ��   transparentButton#��[ 5i      $
��Left=�5j      $	��140��5k      ��5h      �� *ReturnCommand+.��5l      ��  �� Orientation$ 
Horizontal=�6   .+�5m      �� $��Icons\ic_green_back.png��6   $��25��6=   $��25��6K   �� 5n      $��10 0 0 0q�6    Back68   5o      5p      �� 5r      $��Account Management��6   
�������$��	0,0,135,0q�5t      $��20(�5u      $��40��6   $��Center=�5v      $��Center=�62   6Y   5w      �� 5y   
   $��1��6    5z      ��%V   '��       �� 5{      " ��6   .�5|      �� 	 � ��$
 Center=�6<   5}      5~      .@�5      �� $��User Profile��6   $	��350��6,   #��[ 6;   .��5�      �� $��25q�6   .+�5�      �� $��0,25,0,0q�6   $��Images\staff3.jpg��6.   $	��200��6I   $	��200��6Y   � 5�      $��Center=�6   $��75q�68    � ColumnDefinitions 5�      �� 5�      $
��Auto �6/   �� 5�      $��* �6/   .+�5�      �� $��0��6%   $��5q�65   .+�5�      �� $��Regular"�6$   .��69   Name:6A   �� 5�      $��Regular"�6$   .��69   	Position:6D   �� 5�      $��Regular"�6$   .��69   Joining Date:6H   5�      �� 5�      $��1��6%   $��5q�65   .+�5�      �� ��6$   �� *CurrentUser.Name+�� 5�      ��6$   �� *CurrentUser.Position+�� 5�      ��6$   �� *CurrentUser.JoinedDate+5�      5�      5�      5�      �� 5�      $��Change Password��6   $	��350��6/   #��[ 6>   .��5�      � $��25q�6   $��Center=�5�       5�      �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(    5�      �� 5�      $
��Auto �6+   �� 5�      $
��Auto �6+   .+�5�      �� $��Images\passcode.png��6   $��0��5�      $��2��6)   $	��200��5�      $	��200��6)   $��25,25,25,50q�5�      �� 5�      $��1��6   $��0��6)   .��69   Current Passcode:6M   �� 5�      $��2��6   $��0��6)   .��69   New Passcode:6H   �� 5�      $��3��6   $��0��6)   .��69   Confirm Passcode:6L   G� 5�      -   5         G�Name$ CurrentPasscodeBox��5�   "   $��1��5�   "   $��1��6/   G� 5�      -   5        $ NewPasscodeBox��5�   "   $��2��5�   "   $��1��6/   G� 5�      -   5        $ ConfirmPasscodeBox��5�   "   $��3��6>   $��1��5�   "   �� 5�      -   5        $��4��5�      $��2��5�      $��Regular"�5�      $��0,50,0,0q�5�        blackThemeBtn#��[ 5�      $	��250��5�      .��6,   Submit64   5�      5�      �� 5�      $��Account Removal��6   $	��350��6/   #��[ 6>   .��5�      �� $��230,10,150,10q�6   .+�5�      �� $��Images\trashbin.png��6   $	��200��69   $	��200��6E   �� 5�      $��30(�6   $��Fonts\Roboto-Regular.ttf)�6*   .��6P   Delete your account here6j   �� 5�      $��Regular"�6   $��24(�61   
�������.��6Q   We're sorry to see you go.6m   �� 5�      $��24(�6   $��0,45,0,0q�6*   .��6<   Before you go...6N   �� 5�      �� 5�       �� Bullet 5�      A� 5�      $��10��6&   $��10��62   
����p��6=   ._�5�      �� 2  ,System.Windows.Markup.XmlAttributeProperties  XmlSpace preserve5�   .   $��Regular"�5�       $��Fonts\Roboto-Regular.ttf)�5�       $��24(�5�       .��6C   LJ  Account deletion is final. There will be no way to restore your account.6�   5�      �� 5�      $��Regular"�6   $��0,25,0,0q�5�      
�������$��22(�5�      $��Fonts\Roboto-Regular.ttf)�5�      .��6B   (&To continue, enter your passcode below6j   ]� 5�      ��. .+�5�      �� -   5        $��Regular"�5�   !   $��20(�5�   !   $��Right=�5�   !     redThemeBtn#��[ 5�   !   $��25,0,0,0q�5�   !   $��50��5�   !   $	��300��5�   !   .��60   Delete account6@   G� 5�      -   5        $ DeleteAccountPasswordBox��5�   &   5�      5�      5�      5�      5�      5�      