   M S B A M L   `   `   `  ����  Cash Register/    )CashierRegister.Pages.Setting.EditPayment   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      $	��175��5   $      edit_payment  � Title#  �   5	      .��5      � $	��300��6   $��5 20 5 5q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      $��* �6   6+   .+�5   
   ��   
payment_id#���  6   $��Medium"�68   $��Center=�6L   $��0 0 0 10q�6g   6{   �� 5   
   -   5         ��Name$ tblPaymentID��5      $�� ��6*   $��1��62   $��Center=�6B   $��5 0 0 10q�6]   6q   �� 5   
     card#���  6   $��1��62   $��Medium"�6?   $��Center=�6S   6p   �� 5   
   -   5         ��Name$ txbCard��5       �� 	MaxLength$	 200��6#   $��1��63   $��Center=�6@   $��25��6[   $��Center=�6g   $��1��6�   $��5 0 0 0q�6�   6�   �� 5   
   -   5        $ tblNotification��5      
���""��$��2��6D   $��2��6Q   $��Center=�6e   6�   �� 5    
    �� Orientation$ 
Horizontal=�6   $��0��6.   $��2��6>   $��Center=�6R   $��3��6m   $��Right=�6z   $��0 0 0 10q�6�   .+�5!      �� -   5         ��Name$ btnSave��5!        save#���  6<   $��80��6]   6j   �� 5"      -   5        $ 	btnCancel��5"       
 cancel#���  6@   $��80��6c   $��5,0,0,0q�6n   6�   5#      5$      5%      