   M S B A M L   `   `   `  ����I  ERestaurant POS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:FlamyPOSFlamyPOS  5   	       FlamyPOS.TableEditor   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:FlamyPOS     5   	   -   5          � Title$  TablesModify��5   	    � WindowState$ 	Maximized=�6    � WindowStyle$
 None=�66    � 	Resources 5	         watermarkTextBox �� 
TargetType �� Property �� Value �� 
TargetType ��Name
	 � Tag
 ��Name �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName��%�  &	        �� 5
   
   " ��6)   .�5      ��  � ��$ 25��6)   �� 5       � ��$ 50��6*   �� 5       � ��$ 2q�6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� ���#808080$��2q�67   
�������._�5      � .+�5   "   �� $ defaultText��6H   #��z	 6(   $y�Hidden=�5   (   $��12(�5   (   
�������$
��Left=�5   (   $��Center=�6J   �� 5   "   $
 PART_ContentHost��6/   $��5q�6I   5      5       5      ]� 5       5   "   �� 5   &    � ��$  ��6@   �� 5   &    �@J�IsKeyboardFocusWithin .  6Q   ./�5!   "   �� $ defaultText��6)    � y�$ Visible=�6X   5"      5$      5&      5'      5(      .��5)      �  � RowDefinitions 5*   
   �� 5+      $
��Auto �6   �� 5,      $
��Auto �6   �� 5-      $
��Auto �6   �� 5.      $��* �6   .+�50   
   ]� $��40��6   ��. ��52      �� 53      .��54      �  � Color$ #5a5782��6#    � Offset$	 0.2Q�63   � 55      $ MediumPurple��6#   $	 0.6Q�68   � 56      $ Purple��6#   $ 1Q�62   57      .+�5:      �� -   5          transparentButton#��[ 5:      $
��Left=�5<      $	��140��5=      .��5>      ��  �� Orientation$ 
Horizontal=�6   .+�5?      �� $��Icons\ic_green_back.png��6   $��25��6=   $��25��6K   �� 5@      $��10 0 0 0q�6    Back68   5A      5B      �� 5D      -   5        #��[ 5D      $��Right=�5F      $	��250��5G      .��5H      �� $ 
Horizontal=�6   .+�5I      �� Save and Return61   �� 5J      $��Icons\ic_floppy_disk.png��6   $��25��6>   $��25��6L   $��10 0 0 0q�6[   5K      5L      �� 5N      $��Table Layout Editor��6   
�������$��20(�5P      $��40��6   $��Center=�5Q      $��Center=�62   6Y   5R      A� 5T   
    A� Columns$ 5��6   $��2��6"   .+�5U      �� -   5         �� 	IsDefault .   chromeThemeBtn#��[ 5W      .��5X      �� $ 
Horizontal=�6   .+�5Y      �� $��Icons\if_CircledPlus.png��6   $��25��6>   $��25��6L   $��5 0 20 0q�6[   �� 5Z      	Add Table6+   5[      5\      �� 5]      -   5        #��[ 5]   -   .��5^      �� $ 
Horizontal=�6   .+�5_      �� $��Icons\if_information.png��6   $��25��6>   $��25��6M   $��5 0 20 0q�6[   6o   �� 5`      
Check Data6,   5a      5b      �� 5c      $��2��6   $��5 0 0 0q�61   .+�5d      �� $��18(�6   $��Medium"�6&   .��6:   Table Name:6G   �� 5e      -   5         ��Name$ tableNameTextBox��5e      $	 
Table Name��5f      #��[  5g      $��5 0 5 5q�5i      $��30��6+   $��Center=�5j      ��5h      �� *TableNameTextBoxContent+ �� UpdateSourceTrigger$ PropertyChanged=�5j   >   5k      �� 5m      $��Center=�6   $��5 0 0 0q�66   .+�5n      �� $��18(�6   $��Medium"�6&   .��6:   Layout Details6J   �� 5o       �� ContentStringFormat$ Layout Width: {0}��6V   ��6   ��  �� ElementName$ TableLayout�� �� Path$ ActualWidth�6�   �� 5p      $ Layout Height : {0}��6W   ��6   �� $ TableLayout��$ ActualHeight�6�   5q      �� 5s      $��5 0 0 0q�6   $y�Hidden=�6*   .+�5t      �� $��18(�6   $��Medium"�6&   .��6:   Button Dimensions6M   �� 5u      -   5        $ btnHeightTextBox��5u      $	 Height    (Default: 80)��64   #��[  6R   �� 5v      -   5        $ btnWidthTextBox��5v      $	 Width     (Default: 125)��63   #��[  6R   5w      5y      � 5{   
   $��3��6   
�������.+�5|      �� $��3��6   ���#bfbed3���#bfbed3
�������$��2q�6g   $��5 0 5 5q�6{   $��10q�6�   ��5}      �� 5~       �� CornerRadius$ 10��6   ���#5a5782 �� Padding$ 5q�6D   ._�5      �� $��Table Overview��6$   $��25(�6:   
�������6H   5�      .��5�      �� -	   5         ��Name$ TableLayout��5�      5�      5�      5�      5�      5�      