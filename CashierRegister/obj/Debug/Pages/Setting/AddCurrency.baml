   M S B A M L   `   `   `  ����  Cash Register/    )CashierRegister.Pages.Setting.AddCurrency   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5         add_currency  � Title#  �   5	      .��5      � $	��Top=�6   $	��300��6#    � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      $��80 �6   6,   �� 5      $��* �6   6+   .+�5   
   ��   currency#���  6   $��Medium"�66   $��Center=�6J   $��0,0,0,10q�6e   6y   �� 5   
   -   5         ��Name$ txbCurrency��5       �� 	MaxLength$	 200��6'   $��Center=�67   $��25��6R   $��Center=�6^   $��1��6�   $��0,0,0,10q�6�   6�   �� 5   
     tax_rate#���  6   $��Medium"�66   $��Center=�6J   $��1��6e   $��0,0,0,10q�6r   6�   �� 5   
   -   5        $ 
txbTaxRate��5      $��Center=�6R   $��25��6m   $��Center=�6y   $��1��6�   $��1��6�   $��0,0,0,10q�6�   6�   �� 5   
     version#���  6   $��Medium"�65   $��2��6I   $��Center=�6V   $��0,0,0,10q�6q   6�   �� 5   
   -   5        $ 
txbVersion��5      $��1��6&   $��Center=�6/   $��Center=�6J   $��25��6l   $��0,0,0,10q�6x   $��2��6�   $��1��6�   6�   �� 5   
   -   5         ��Name$ 	chkActive��5      $��Medium"�6&   $��Center=�6:   $��Center=�6U    
 active#���  6w   $��3��6�   $��0,0,0,10q�6�   6�   �� 5!   
    �� Orientation$ 
Horizontal=�6   $��0��6.   $��2��6>   $��Center=�6R   $��4��6m   $��Right=�6z   $��0,0,0,20q�6�   .+�5"      �� -   5         ��Name$ btnAdd��5"        add#���  6:   $��80��6Z   6g   �� 5#      -   5        $ 	btnCancel��5#       
 cancel#���  6@   $��80��6c   $��5,0,0,0q�6n   6�   5$      �� 5%   
   -   5         ��Name$ tblNotification��5%      
���""��$��5��6D   $��2��6Q   $��Bottom=�6e   6�   5&      5'      