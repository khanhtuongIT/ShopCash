   M S B A M L   `   `   `  ����H  DCash Register, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:CashierRegister.Pages.StatisticCashierRegister.Pages.Statistic  5      *    $CashierRegister.Pages.Statistic.Home   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:CashierRegister.Pages.Statistic     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5        .��5
      �    ContentRoot#��[  6   $��0q�60   .+�5   
   =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  -   5           Name$	  mpr��5   !   $	��100��6.   $	��100��6:   $��0 0 0 56q�6G      IsActive .    CircleProgressRingStyle#��[ 6j   6�   � 5   
   -   5         �Name$ 
grdContent��5       � RowDefinitions 5      �� 5      $��45 �6    6.   �� 5      $��* �6    6-   .+�5      � 
������� � ColumnDefinitions 5      �� 5      $
��Auto �6'   66   �� 5      $
��Auto �6'   66   �� 5      $��* �6'   63   �� 5      $
��Auto �6'   66   .+�5      �� -   5         ��Name$ cboCategory��5       �� IsTextSearchEnabled . $��Center=�6K   $��25��6f   $	��100��6r   $��5 0 0 0q�6�    �� SelectedValuePath$ 
CategoryID��6�    �� DisplayMemberPath$ CategoryName��6�   ��6�   �� 6  � 5      $��1��6   $��Center=�6'   $��25��6B    5      �� 5      $
��Auto �6+   6:   �� 5      $
��Auto �6+   6:   �� 5      $
��Auto �6+   6:   �� 5       $
��Auto �6+   6:   �� 5!      $
��Auto �6+   6:   �� 5"      $
��Auto �6+   6:   .+�5$      ��   year#���  6    $��25 0 5 0q�6>   $��Center=�6P   6m   �� 5%      -   5        $ cboYear��5%       . $	��100��6K   $��1��6Z   $
 Year��6j   $
 Year��6�   6�   �� 5'       	 month#���  6    $��10 0 5 0q�6?   $��2��6Q   $��Center=�6a   6~   �� 5(      -   5        $ cboMonth��5(       . $	��100��6L   $��3��6[   $ Month��6k   $ Month��6�   6�   �� 5*        day#���  6    $��10 0 5 0q�6=   $��4��6O   $��Center=�6_   6|   �� 5+      -   5        $ cboDay��5+       . $	��100��6J   $��5��6Y   $	 Day��6i   $	 Day��6�   6�   5,      �� 5.      -   5        	 ��Name$	 tblTotal��5.      $��2��6.     
total_zero#���  6>     Heading2#��[ 6b   
���""��$��Center=�6�   $��Center=�6�   6�   �� 50      $��3��6   $��Center=�6-   
 �� Orientation$
 
Horizontal=�6H   $��0,0,5,0q�6a   .+�51      �� -	   5         ��Name$ 	txbSearch��51   6   $	��150��6   $��25��6*   $��0,0,3,0q�6I   $��Center=�6Z   6�   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  52      -
   5          Name$ muiBtnSearch��52   '      EllipseDiameter$ 30Q�6X      
IconHeight$ 15Q�6m      	IconWidth$ 15Q�6}      IconData� ���|h��T��SF0�T��	��Ȯb(#���"��*`@����p�a�q2�l��0"��Ε��Mb��@?��r ��F�H�! |Q�K�K��:��c�h�:UY�)��T�|h��T���O   �8��Y���@X�#�qp��?�!d�+ ��!�Q�`�)��
��(��Ө�*(����&�m��d^$w�`G�1H��+��r�3��-r3�p�0��f1H2Ә�[/���3x�M,�Xi3 j�*��1р�� ��"�Ps��C'$P�м$��HH�g$�(sh]b#lt����m� �bgӄ�,�.Hz
�c  ��O   	 � ToolTip 53      �� 54       
 search#���  6(   6J   56      57      58      &   System.Windows.Controls.DataGrid  5:      -   5          Name$ dtgStatistic��5:      $y�Visible=�6-   $��1��6B      HeadersVisibility$ Column=�6O      
IsReadOnly .    AutoGenerateColumns .     SelectionMode$ Single=�6�      Columns 5;      4  .System.Windows.Controls.DataGridTemplateColumn  5H      ,  &System.Windows.Controls.DataGridColumn   Header 5I      �� 5J        product_name#���  6(   6P      CellTemplate 5L      �� 5M      .R�5N   "   �� $��Center=�6F   $��0 5 0 5q�6a   ��6,   �� *
SortName+6t   5O      5Q        5S       5T      �� 5U       	 category_name#��� 	 6(   6Q    5W      �� 5X      .R�5Y   "   �� $��Center=�6J   ��6,   �� *CategoryName+6g   5Z      5\        5^       5_      �� 5`       
 sold_quantity#��� 
 6(   6Q    5b      �� 5c      .R�5d   "   �� $��Center=�6J   $��Right=�6e   ��6,   �� *SoldQuantity+6�   5e      5g        5i       5j      �� 5k        quantity_available#���  6(   6V    5m      �� 5n      .R�5o   "   �� $��Center=�6O   $��Right=�6j   ��6,   �� *QuantityAvailable+6�   5p      5r      5u      5v      5w      5x      