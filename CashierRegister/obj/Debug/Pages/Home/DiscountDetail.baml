   M S B A M L   `   `   `  ����  Cash Register/    )CashierRegister.Pages.Home.DiscountDetail   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5           discount_detail  � Title#  �   5	      .��5      � $	��Top=�6   $	��350��6#    � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   .+�5   
   ��   please_choose_following#���  6   $��Center=�6E   $��0,0,0,10q�6`   6t   � 5   
   $��1��6   $��35��6   $��0,0,0,15q�6(    � ColumnDefinitions 5      �� 5      6%   �� 5      6%   .+�5      �� -   5         ��Name$ 	btnAmount��5       
 amount#���  6@   
�������$��0,0,1,0q�6v   $
��Bold"�6�   $��0q�6�   ��5      �� 5       �� 
TargetType" ��6&   .R�5       ��  ��Name$ amount��6!   #��z��61   ._�5!      �� $��Center=�6/   $��Center=�6J   $��Content��6g   6�   5"       �� Triggers 5#      P� 5$       P� Property �@J�IsMouseOver P� Value . . �5%   "   �� 	 �� 
TargetName$	 amount��6)   
 �� Property
 � �� �� Value �	#FF2b60176g   5&      5(      5*      �� 5+      -   5        $ 
btnPercent��5+        percent#���  6B   
�������$��1,0,0,0q�6y   $
��Bold"�6�   $��1��6�   $��0q�6�   ��5,      �� 5-      " ��6&   .R�5.      �� $ percent��6!   #��z��62   ._�5/      �� $��Center=�6/   $��Center=�6J   $��Content��6g   6�   50       51      P� 52       �@J�IsMouseOver . . �53   "   �� $	 percent��6)   
 � �� �	#FF2b60176h   54      56      58      59      � 5;   
   $��2��6   $��0,0,0,30q�6    5<      �� 5=      $	��150 �6#   61   �� 5>      $��* �6#   6/   .+�5@      �� -   5         ��Name$ tblAmountPercent��5@      $��Center=�62   $
��Bold"�6M   6a   �� 5A      -   5        $ tblDiscountDetailType��5A      $��Center=�67   $��Right=�6R   6p   �� 5B      -   5         ��Name$ txbAmountPercent��5B       �� 	MaxLength$ 12��60   $��Center=�6t   $��25��6�   $��Center=�6�   $��1��6�   $��5,0,0,0q�6�   6  5C      �� 5E   
    �� Orientation$ 
Horizontal=�6   $��3��6.   $��Right=�6;   $��Bottom=�6W   .+�5F      �� -   5        $ btnOK��5F        ok#���  68   $��80��6W   6g   �� 5G      -   5        $ 	btnCancel��5G       
 cancel#���  6@   $��80��6c   $��5,0,0,0q�6q   6�   5H      �� 5J   
   -	   5        $ tblNotification��5J      $��0,20,0,0q�6-   
���""��$��Bottom=�6V   $��4��6q   6�   5K      5M      