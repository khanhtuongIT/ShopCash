   M S B A M L   `   `   `  ����  Cash Register/    )CashierRegister.Pages.Setting.BackupLocal   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5        .��5	      �    ContentRoot#��[  6   $��0q�60     � RowDefinitions  5
   
   �� 5      $��45 �6   6*   �� 5      $��* �6   6)   .+�5   
   � 
������� � ColumnDefinitions 5      �� 5      $	��100 �6#   61   �� 5      $��* �6#   6/   .+�5      7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  -   5          Name$ muiBtnBackup��5      $y�Visible=�6P      EllipseDiameter$ 35Q�6e      
IconHeight$ 20Q�6z      	IconWidth$ 20Q�6�   $��4,0,0,0q�6�   $��Center=�6�      IconData� ��qS��їqS��\�e��\�e����0����0��\�B�A��\�B�A����������\�`k��\�`k���tE)!X	@@���tE)!X	@@��\�c^G��@@��\�c^G��@@���a
����a
���\�ѵ���\�ѵ���𙯢��љ����\��^���\��^�����T����T��\�(���\�(������[�����[��\�y�m��\�y�m����8����8��\љNJ��\љNJ���aH���aH��\ѹ�&��\ѹ�&������ ��р�� ��\����\������ ��ѹ�� ��\�)B� ��\�)B� ���
8� ���
8� ��\�`� ��\�`� ���ŧ�^\�!@���ŧ�^\�!@��\р�� ��\р�� ����H��"[@����H��"[@��\��|F}��@��\��|F}��@���b ��@���b ��@��\����Q�@��\����Q�@�����0~/@a�a���0~/@ ��ј^k ��ј^ka�a���0~/@�#����0~/@/Pј^k/Pј^k�#����0~/@��� ���0~/@I�Xј^kI�Xј^k��� 0��0~/@�|F}��@���0~/@`�� ј^k`�� Q�^k�|F}��@�        ѦX�    ѦX�˿���˿��� ����!P ����!P˿�������@˿�������@ ����z�����? ����z�����?˿��    ˿�	 � ToolTip 5      �� 5        backup_database#���  6    6K   5      �� 5      -   5         ��Name$ tblTotal��5        
total_zero#���  6*     Heading2#��[ 6N   
���""��$��Center=�6�   $��Center=�6�   $��1��6�   6�   5      =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  5   
   -   5        	  Name$		 mpr��5   !   $��0,0,0,56q�6.   
   IsActive
 .  $��Center=�6Q   $��Center=�6l   $	��100��6�   $	��100��6�     CircleProgressRingStyle#��[ 6�   $��1��6�   6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dtgDatabase��5         HeadersVisibility$ Column=�6(   $��1��6C   $y�Visible=�6P      
IsReadOnly .    AutoGenerateColumns .     Columns 5       4  .System.Windows.Controls.DataGridTemplateColumn  5-      ,  &System.Windows.Controls.DataGridColumn   Header 5.      �� 5/        backup_date#���  6$   6K      CellTemplate 51      �� 52      .R�53      �� $��Center=�6I   ��6(   ��  �� Path$ 
BackupDate�6f   54      56        58       59      �� 5:        	file_size#���  6$   6I    5<      �� 5=      .R�5>      �� $��Center=�6G   ��6(   �� $ FileSize�6d   5?      5A        5C       5D      �� 5E       
 delete#���  6$   6F    5G      �� 5H      .R�5I      �� -   5        $��0q�5I   =   
������ $��Center=�6j   $
��Left=�6�   .��5J   "   �� $��18��6S   $��18��6^   $��Center=�6j   ��6(   �� $ BitmapImage_Delete�6�    5K   "   �� 5L   &     delete_database#���  60   6[   5N      5O      5Q        5S       5T      �� 5U       	 restore#��� 	 6$   6G    5W      �� 5X      .R�5Y      �� -   5        $��0q�5Y   >   
������ $
��Left=�6k   .��5Z   "   �� $��20��6T   $��20��6_   $��Center=�6k   ��6(   �� $ BitmapImage_Restore�6�    5[   "   �� 5\   &    
 restore_database#��� 
 60   6\   5^      5_      5a      5d      5g      5h      