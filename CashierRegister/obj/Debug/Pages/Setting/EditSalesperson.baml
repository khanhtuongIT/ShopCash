   M S B A M L   `   `   `  ����  Cash Register3    -CashierRegister.Pages.Setting.EditSalesperson   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5         edit_salesperson  � Title#  �   5	      .��5      � $��Center=�6   $	��400��6&    � ColumnDefinitions 5   
   �� 5      $	��120 �6   6-   �� 5      $��* �6   6+    � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   .+�5   
   ��   salesperson_id#���  6   $��Center=�6<   $��Medium"�6W   $��0,0,0,10q�6k   6   �� 5   
   -   5         ��Name$ tblSalespersonID��5      $��1��6.   $��25��6>   $��Center=�6J   $�� ��6e   $��0,0,0,10q�6m   6�   �� 5!   
     salesperson_name#���  6   $��Center=�6>   $��1��6Y   $��Medium"�6f   $��0,0,0,10q�6z   6�   �� 5"   
   -   5         ��Name$ txbName��5"       �� 	MaxLength$	 200��6#   $��1��63   $��1��6C   $��25��6P   $��Center=�6\   $��Center=�6w   $��0,0,0,10q�6�   6�   �� 5$   
     birthday#���  6   $��Center=�66   $��Medium"�6Q   $��2��6e   $��0,0,0,10q�6r   6�   (  "System.Windows.Controls.DatePicker  5%   
   -   5          Name$ dtpBirthday��5%      $��25��6*   $��Center=�66   $��Center=�6Q   $��1��6s   $��2��6�   $��0,0,0,10q�6�   6�   �� 5'   
     address#���  6   $��Center=�65   $��Medium"�6P   $��3��6d   $��0,0,0,10q�6q   6�   �� 5(   
   -   5        $ 
txbAddress��5(      $	 250��6&   $��1��66   $��25��6F   $��Center=�6R   $��Center=�6m   $��3��6�   $��0,0,0,10q�6�   6�   �� 5*   
    	 email#���  6   $��Center=�63   $��Medium"�6N   $��4��6b   $��0,0,0,10q�6o   6�   �� 5+   
   -   5        $ txbEmail��5+      $	 100��6$   $��1��64   $��25��6D   $��Center=�6P   $��Center=�6k   $��4��6�   $��0,0,0,10q�6�   6�   �� 5-   
     password#���  6   $��Center=�66   $��Medium"�6Q   $��5��6e   $��0,0,0,10q�6r   6�   G� 5.   
   -   5         G�Name$ pwbPassword��5.       G� 	MaxLength$	 100��6+   $��25��6;   $��Center=�6G   $��Center=�6b   $��1��6�   $��5��6�   $��0,0,0,10q�6�   6�   �� 50   
    
 active#���  6   $��Center=�64   $��Medium"�6O   $��6��6c   $��0,0,0,10q�6p   6�   �� 51   
   -   5        	 ��Name$	 	chkActive��51      $��1��6&   $��25��66   $��Center=�6B   $��Center=�6]   $��6��6   $��0,0,0,10q�6�   6�   �� 53   
   
 �� Orientation$
 
Horizontal=�6   $��7��6.   $��1��6;   $��Right=�6K   $	��Top=�6g   .+�54      �� -   5         ��Name$ muiBtnOK��54      $��80��6>     ok#���  6I   $��0,0,5,0q�6h   6{   �� 55      -	   5        $ muiBtnCancel��55      $��80��6F    
	 cancel#��� 	 6Q   6v   56      �� 57   
   -
   5        $ tblNotification��57      $��0,20,0,0q�6-   
���""��$�� ��6V   $��Bottom=�6^   $��8��6y   $��2��6�   6�   58      59      