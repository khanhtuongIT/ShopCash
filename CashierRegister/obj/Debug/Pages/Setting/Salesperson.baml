   M S B A M L   `   `   `  ����  Cash Register/    )CashierRegister.Pages.Setting.Salesperson   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      -   5        .��5	      �    ContentRoot#��[  6   $��0q�61   .+�5
   
   =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  -   5           Name$	  mpr��5
   !   $	��100��6.   $��0,0,0,56q�6:   $	��100��6L      IsActive .    CircleProgressRingStyle#��[ 6j   6�   � 5   
   -   5         �Name$ 	grContent��5       � RowDefinitions 5      �� 5      $��45 �6    6.   �� 5      $��* �6    6-   .+�5      � 
������� � ColumnDefinitions 5      �� 5      $	��100 �6'   65   �� 5      $��* �6'   63   �� 5      $	��200 �6'   65   .+�5      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�66   $��2,0,0,0q�6Q   .+�5      7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  -   5          Name$ muiBtnDelete��5   '      EllipseDiameter$ 35Q�6X      
IconHeight$ 20Q�6m   	   	IconWidth$	 20Q�6}   $��0,0,10,0q�6�   $��Center=�6�   
   IconData�
 ���)���?
��)���?(�8O"��?(�8O"��?
�8���?
�8���?(��nu��?(��nu��?
���    ����!    ����!Dt����n.Dt����n.��?
�PΎ*��?
�PΎ*��n.�8����n.�8����?
�    ��?
�    Dt����Dt�	 � ToolTip 5      �� 5        delete_salesperson#���  6(   6V   5        5      -   5        $ 	muiBtnAdd��5   '   $ 35Q�6R   $ 20Q�6g   $	 20Q�6w   $��2��6�   $��Center=�6�   $��Center=�6�   �
 ��.    �㥛� @@@    �㥛� @@@��.Ѱ���.Ѱ����㥛� @@@���㥛� @@@ȉѨ�.ȉѨ�.���    ���    ��.Ѩ�.��.	 5      �� 5         add_salesperson#���  6(   6S   5"      5#      �� 5%      -   5         ��Name$ tblTotal��5%        Heading2#��[ 6.   $��1��6P   $��Total(0)��6`   $
��Bold"�6p   
���""��$��Center=�6�   $��Center=�6�   6�   �� 5'      $��2��6   $��Center=�6-   $ 
Horizontal=�6H   $��Right=�6a   $��0,0,5,0q�6}   .+�5(      �� -   5         ��Name$ 	txbSearch��5(      $	��150��61   $��0,0,3,0q�6=   $��Center=�6N   6�     5)      -   5        $ muiBtnSearch��5)   '   $ 30Q�6X   $ 15Q�6m   $	 15Q�6}   �
 ���|h��T��SF0�T��	��Ȯb(#���"��*`@����p�a�q2�l��0"��Ε��Mb��@?��r ��F�H�! |Q�K�K��:��c�h�:UY�)��T�|h��T���O   �8��Y���@X�#�qp��?�!d�+ ��!�Q�`�)��
��(��Ө�*(����&�m��d^$w�`G�1H��+��r�3��-r3�p�0��f1H2Ә�[/���3x�M,�Xi3 j�*��1р�� ��"�Ps��C'$P�м$��HH�g$�(sh]b#lt����m� �bgӄ�,�.Hz
�c  ��O   	 5*      �� 5+       
 search#���  6(   6J   5-      5.      50      &   System.Windows.Controls.DataGrid  53      -	   5          Name$ dtgSalesperson��53      $y�Hidden=�6b      HeadersVisibility$ Column=�6v   $��1��6�      AutoGenerateColumns .     
IsReadOnly .    SelectionMode$ Single=�6�      Columns 54      4  .System.Windows.Controls.DataGridTemplateColumn  55      ,  &System.Windows.Controls.DataGridColumn   Header 56      �� 57      -
   5         ��Name$ chkCheckAll��57   '    58   "   �� 59   &     select_all_salesperson#���  60   6b   5;         CellTemplate 5=      �� 5>      .R�5?   "   �� -   5        $��Center=�5?   O   $��4,0,0,0q�6�    d� 	IsChecked 6+   ��  �� Path$ CheckDel� 5@   &   �� 5A   *     select_this_salesperson#���  64   6g   5C   #   5D      5F        5S       5T      �� 5U        salesperson_name#���  6(   6T    5W      �� 5X      .R�5Y   "   �� $��Center=�6G   ��6,   �� $
 Name�6d   5Z      5\        5^       5_      �� 5`       	 birthday#��� 	 6(   6L    5b      �� 5c      .R�5d   "   �� $��Center=�6K   ��6,   �� $ Birthday�6h   5e      5g        5i       5j      �� 5k       
 address#��� 
 6(   6K    5m      �� 5n      .R�5o   "   �� $��Center=�6J   ��6,   �� $ Address�6g   5p      5r        5t       5u      �� 5v       	 email#���  6(   6I    5x      �� 5y      .R�5z   "   �� $��Center=�6H   ��6,   �� $ Email�6e   5{      5}        5       5�      �� 5�       
 active#���  6(   6J    5�      �� 5�      .R�5�   "   �� $��15,0,0,0q�6+   {�.  $��Center=�6O    6j   �� $ Active�6�   5�      5�        5�       5�      �� 5�        edit#���  6(   6H    5�      �� 5�      .R�5�   "   �� -   5        $��Center=�5�   ?   $��Center=�6Z   $��0q�6w   
������ 
������ .��5�   &   �� $��18��6M   $��18��6X   ��6,   �� $ ImageUrl�6f    5�   &   �� 5�   *     edit_salesperson#���  64   6`   5�   #   5�      5�      5�      5�      5�      5�      