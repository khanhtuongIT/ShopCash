   M S B A M L   `   `   `  ����  Cash Register0    *CashierRegister.Pages.Setting.EditCurrency   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5         edit_currency  � Title#  �   5	      .��5      � $	��Top=�6   $	��300��6#    � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      $	��100 �6   6-   �� 5      $��* �6   6+   .+�5   
   ��   currency_id#���  6   $��Medium"�69   $��Center=�6M   $��0,0,0,10q�6h   6|   �� 5   
   -   5         ��Name$ tblSettingID��5      $��Center=�6*   $��0,0,0,10q�6E   $��1��6W   6i   �� 5   
     currency#���  6   $��Medium"�66   $��Center=�6J   $��1��6e   $��0,0,0,10q�6r   6�   �� 5   
   -   5         ��Name$ txbCurrency��5       �� 	MaxLength$	 200��6'   $��1��67   $��Center=�6G   $��25��6i   $��1��6u   $��0,0,0,10q�6�   6�   �� 5    
     tax_rate#���  6   $��Medium"�66   $��Center=�6J   $��2��6e   $��0,0,0,10q�6r   6�   �� 5!   
   -   5        $ 
txbTaxRate��5!      $��25��6R   $��Center=�6^   $��2��6y   $��1��6�   $��0,0,0,10q�6�   6�   �� 5#   
     version#���  6   $��Medium"�65   $��3��6I   $��Center=�6V   $��0,0,0,10q�6q   6�   �� 5$   
   -   5        $ 
txbVersion��5$      $��25��6&   $��Center=�62   $��0,0,0,10q�6T   $��3��6f   $��1��6s   6�   �� 5&   
   -   5         ��Name$ 	chkActive��5&      $��Medium"�6&   $��Center=�6:    
 active#���  6U   $��Center=�6x   $��4��6�   $��0,0,0,10q�6�   6�   �� 5(   
    �� Orientation$ 
Horizontal=�6   $��0��6.   $��2��6>   $��Center=�6R   $��5��6m   $��Right=�6z   $��0,0,0,20q�6�   .+�5)      �� -   5         ��Name$ btnAdd��5)        ok#���  6:   $��80��6Y   6f   �� 5*      -   5        $ 	btnCancel��5*       
 cancel#���  6@   $��80��6c   $��5,0,0,0q�6n   6�   5+      �� 5,   
   -   5        $ tblNotification��5,      
���""��$��6��6D   $��2��6Q   $��Bottom=�6e   6�   5-      5.      