   M S B A M L   `   `   `  ����  Cash Register,    &CashierRegister.UserControls.UCProduct   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      �� $��0q�5	   	   
���   �$	��175��61   $	��130��6=     �� CornerRadius$  3 3 3 3��5
   	   $��10q�6     J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ Black��6      	Direction$	 225Q�6.      
BlurRadius$	 6.5Q�6>      ShadowDepth$	 0.5Q�6O   6c   ._�5   
   �  J� OpacityMask 5      !� 5       j� Stretch$
 None=�6F    !� Visual 6   �� 	 �� ElementName$	 clipMask��6F   .+�5      �� -   5        
 ��Name$
 clipMask��5      
�������$��0q�6:   $  3 3 3 3��6N   � 5       � RowDefinitions 5      �� 5      $��* �6$   61   �� 5      $
��Auto �6$   64   �� 5      $��25 �6$   62   .+�5      � 
������ .+�5      �� $	��Top=�6   $��Center=�64   $��Uniform=�6u   ��6Q   ��  �� Path$ BitmapImage�6�   � 5      $	��Top=�6   $��Right=�63   .+�5      �� $��45��6N   $��32��6Y   $
��Fill=�6e   $��Center=�6t   $��0 3 3 0q�6�   ��6    �� $ Image_Iventory_Status�6�   �� 5      $��0 2 3 0q�6I   
�������$��Medium"�6m   $��Center=�6�   $��Center=�6�   ��6$   �� $ InventoryCount�6�   5      5      �� 5!      $��1��6   ���	#FF6161616B   � 5#      
������ $��Center=�60   $��2��6K    � ColumnDefinitions 5$      �� 5%      $��* �6+   67   �� 5&      $
��Auto �6+   6:   .+�5(      �� $��Medium"�6C   $��3 0 0 0q�6W   $
��Left=�6h   ��6    �� $ StrShortName�6�   �� 5*      $��0 0 3 0q�6?   $��1��6P   $��Right=�6`   $��Medium"�6|   
���""����6    �� $ StrPrice�6�   5+      ��5,      �� 5-       �� 
TargetType" �6   .�5.      ��  �� Property � �� �� Value
 ���� 6M    �� Triggers 5/      P� 50       P� Property �@J�IsMouseOver P� Value . . �51   "   ��  � ��
 �����6T   52      54      56      58      59      5:      