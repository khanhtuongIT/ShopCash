   M S B A M L   `   `   `  ����  Cash Register2    ,CashierRegister.Pages.Setting.ChangePassword   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5        .��5	      �    ContentRoot#��[  6   $��0q�60   $	��Top=�6;   $	��355��6S   $
��Left=�6b     � RowDefinitions  5
   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      $��* �6   6+   .+�5   
   ��   change_password#���  6   
����iA�$��2��6T     Heading1#��[ 6h   6�   �� 5   
     current_password#���  6   $��Medium"�6>   $��1��6R   $��0 20 0 0q�6_   $��Center=�6q   6�   G� 5   
   -   5         G�Name$ pwbCurrentPassword��5       G� 	MaxLength$	 100��6W   $��1��6g   $��25��6w   $��Center=�6�   $��1��6�   $��3 20 0 0q�6�   $��Center=�6�   6�   �� 5   
     new_password#���  6   $��Medium"�6:   $��2��6N   $��0 5 0 0q�6[   $��Center=�6l   6�   G� 5   
   -   5        $ pwbNewPassword��5      $	 100��6S   $��1��6c   $��25��6s   $��Center=�6   $��2��6�   $��3 5 0 0q�6�   $��Center=�6�   6�   �� 5   
     confirm_password#���  6   $��Medium"�6>   $��3��6R   $��0 5 0 0q�6_   $��Center=�6p   6�   G� 5   
   -   5        $ pwbConfirmPassword��5      $	 100��6W   $��25��6g   $��Center=�6s   $��1��6�   $��3��6�   $��3 5 0 0q�6�   $��Center=�6�   6�   �� 5!   
    �� Orientation$ 
Horizontal=�6   $��4��6.   $��0 10 0 0q�6;   $��Right=�6M   $��2��6i   .+�5"      �� -   5         ��Name$ btnOK��5"      $��80��6$     ok#���  6F   6g   �� 5#      -   5        $ 	btnCancel��5#       
 cancel#���  6(   $��5 0 0 0q�6c   $��80��6t   6�   5$      �� 5&   
   -   5         ��Name$ tblNotification��5&      
���""��$��0 15 0 0q�6D   $��5��6V   $��2��6c   6y   5(      5)      