   M S B A M L   `   `   `  ����  Cash Register4    .CashierRegister.Pages.Setting.UndoRestoreLocal   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5        .��5	      �    ContentRoot#��[  6   $��0q�60     � RowDefinitions  5
   
   �� 5      $��45 �6   6*   �� 5      $��* �6   6)   .+�5   
   � 
�������.+�5      �� -   5         ��Name$ tblTotal��5        
total_zero#���  6*     Heading2#��[ 6N   
���""��$��Center=�6�   $��Center=�6�   $��1��6�   6�   5      =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  5   
   -   5          Name$	 mpr��5   !   $��0,0,0,56q�6.      IsActive .  $��Center=�6Q   $��Center=�6l   $	��100��6�   $	��100��6�     CircleProgressRingStyle#��[ 6�   $��1��6�   6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dtgDatabase��5         HeadersVisibility$ Column=�6(   $��1��6C   $y�Visible=�6P      
IsReadOnly .    AutoGenerateColumns .     Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5"      ,  &System.Windows.Controls.DataGridColumn	   Header	 5#      �� 5$        backup_date#���  6$   6K   
   CellTemplate
 5&      �� 5'      .R�5(      �� $��Center=�6I   ��6(   ��  �� Path$ 
BackupDate�6f   5)      5+        5-      	 5.      �� 5/        	file_size#���  6$   6I   
 51      �� 52      .R�53      �� $��Center=�6G   ��6(   �� $ FileSize�6d   54      56        58      	 59      �� 5:       
 delete#���  6$   6F   
 5<      �� 5=      .R�5>      �� -   5        $��0q�5>   =   
������ $��Center=�6j   $
��Left=�6�   .��5?   "   �� $��18��6S   $��18��6^   $��Center=�6j   ��6(   �� $ BitmapImage_Delete�6�    � ToolTip 5@   "   �� 5A   &     delete_database#���  60   6[   5C      5D      5F        5H      	 5I      �� 5J        restore#���  6$   6G   
 5L      �� 5M      .R�5N      �� -   5        $��0q�5N   >   
������ $
��Left=�6k   .��5O   "   �� $��20��6T   $��20��6_   $��Center=�6k   ��6(   �� $ BitmapImage_Restore�6�    5P   "   �� 5Q   &    	 restore_database#��� 	 60   6\   5S      5T      5V      5Y      5\      5]      