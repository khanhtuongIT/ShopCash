   M S B A M L   `   `   `  ����H  DCash Register, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:CashierRegister.ViewsCashierRegister.Views  5      E'clr-namespace:CashierRegister.ViewModelCashierRegister.ViewModel  5      A%clr-namespace:CashierRegister.HelpersCashierRegister.Helpers  5	      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5      $    CashierRegister.Views.ShellOut   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      /local#clr-namespace:CashierRegister.Views  5      7	viewModel'clr-namespace:CashierRegister.ViewModel  5      /vml%clr-namespace:CashierRegister.Helpers  5	      XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5      -   5        .   (CashierRegister.Helpers.ViewModelLocator    AutoHookedUpViewModel  . $
��9999��5   >   $
��9999��6N      
single_pay � Title# �   5       � 	Resources 5        studentsTemplate �� Orientation ��Name �� Path �� Mode7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  Name   EllipseStrokeThickness	   
IconHeight
   	IconWidth   IconData
 J� Uid �� RelativeSource �� AncestorType �� ElementName � ToolTip  del_this_payment �� TextAlignment  select_then_press��%l  &	       �� 5   
   .R�5      �� $ 
Horizontal=�6   .+�5      �� $ txtPaymentName��6   $��16(�64   $��0��6u   $	��140��6�   $��0,5,10,7q�6�   ��6B   �� $ PaymentName�$ TwoWay=�5        5      $ btDelete��6=   
�������$ 0Q�6n   $	 16Q�6�   $
 16Q�6�   $��Center=�6�   $��Center=�6�   � ���kcG>
 @� �d��W�@� �d��W�@6���ذK��#@6���ذK��#@� ѫ� � ѫ� 6��ѵ16��ѵ1� ��hP� ��hP H���kcG>
 @ H�p    ��x 03@Qo��x 03@Qo/��/�@Q    /��/�@�E�8m@    ��    Q����O�@E�8m@���O�@	$��	-20,0,0,0q�5       5   #   �� *	PaymentId+y�5   2   �� *ShowButtonDel+$ OneTime=���6e   �� $! DataContext.btDelPaymentCmd� �� *FindAncestor+" ����6�   �� *Uid+$ btDelete�� 5      �� 5      #���  6$   6P   5      �� 5      $ txtPaymentBalance��6   $��16(�67   $��Right=�6{   $��1��6�   $ Right=�6�   $	��150��6�   $��0,5,0,7q�6�   ��6E   �� $ PaymentBalance�$ TwoWay=�6�    5      �� 5      #���  6    6M   5!      5"   	   5#      .  (System.Windows.Interactivity.Interaction   Triggers 5$      /  )System.Windows.Interactivity.EventTrigger  5%   
      	EventName$ Unloaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5&      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *unloadedCmd+6F   5'      .��5)      � -   5         �Name$ grdMain��5)      $	��900��6    � RowDefinitions 5*   
   �� 5+      $��* �6   6)   �� 5,      $
��Auto �6   6,    � ColumnDefinitions 5.   
   �� 5/      $	��320 �6   6-   �� 50      6!   .+�52   
   � $��0,0,0,20q�6    53      �� 54      6"   �� 55      $
��Auto �6    60   �� 56      $
��Auto �6    60   �� 57      $
��Auto �6    60   .+�5:      � $��0��6   .+�5;      �� -   5         ��Name$ lstShellOut��5;   �   #��[ 6C   $��Hidden=�6  ��6   �� $ ShellOut� �� SelectedItem 6t   �� *SelectedShellOut+ �� SelectedIndex 6�   �� *SelectedIndex+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� 5<        5=      $ SelectionChanged��6)   . 5>         64   �� $ DataContext.MouseDownCmd� �� *FindAncestor+" ��   CommandParameter 6�   �� *SelectedItem+$ lstShellOut��6�   5?        5@      $ MouseDoubleClick��6)   . 5A         64   �� $% DataContext.MouseDoubleClickCmd� �� *FindAncestor+" �� 6�   �� *SelectedItem+$ lstShellOut��6   5B      5D      5E      �� 5F      $��3��6   $��0��6,   
�������$��Bottom=�6Q   $��0,20,0,0q�6l   6�   � 5H      $��1��6   $��	0,5,0,100q�6    5S      �� 5T      $��3��6   $��1��6,   
�������$��Bottom=�6Q   6n   �� 5V      $��2��6    	 total#���  6%   $��Center=�6D   $��16(�6_   $��5,10,0,5q�6m   $��Medium"�6   6�   �� 5W      $��2��6   $ 
Horizontal=�6&   $��Right=�6?   $��Center=�6[   $��	0,10,10,5q�6v   .+�5X      �� -   5        $ tblTotalUnit��5X      $��18(�62   $�� ��6@   $
��Bold"�6H   $��Center=�6Z   6w   �� 5Y      -   5        $ tblTotal��5Y      $��20(�6.   $
��Bold"�6<   $��Center=�6N   6l   5Z      �� 5[      $��3��6   
�������$��2��6D   $��Bottom=�6Q   6o   �� 5]      $��3��6     cash_balance#���  6%   $��Center=�6K   $��16(�6f   $��5,10,0,9q�6t   $��Medium"�6�   $
��Left=�6�   $	��475��6�   6�   �� 5^      $��3��6   $ 
Horizontal=�6&   $��Right=�6?   $��Center=�6[   $��0,0,10,0q�6v   .+�5_      �� -   5        $ tblCashBalanceUnit��5_      $��17(�68   
���""��$�� ��6]   $
��Bold"�6e   $��Center=�6w   6�   �� 5`      -   5        $ tblCashBalance��5`      $��19(�64   
���""��$��DemiBold"�6Y   $��Center=�6o   ��6�   �� *
txtTotal+$ PropertyChanged=�$ TwoWay=�6�   5a      �� 5b      $��3��6   $��3��6,   
�������$��Bottom=�6Q   6o     5c      -   5        $ mainButtonAdd��5c   �      EllipseDiameter$ 30Q�6   $	 20Q�6�   $
 20Q�6�   $��Right=�6�   $	��Top=�6�   s ���     �	9/    �	9/ � � H� � � H��</�	9/�</�	9/D��� D��� �</�    �</�     � ��  � 	$��0, -50, 0, 0q�5d      $ 2Q�6\   y�5c      �� *IsShowButtonAddPayment+$ TwoWay=�$ PropertyChanged=���5d   5   �� *AddShellOutCommand+ 5f      �� 5g        add_other_payment#���  6    6M   5i      5j      � 5l   
   $��1��6   $��20,0,0,0q�6    5m      �� 5n      $
��Auto �6    60   �� 5o      $��* �6    6-    5q      �� 5r      -	   5         ��Name$ column1��5r   #   $��2* �64   6A   �� 5s      -
   5        $ column2��5s   #   $��3* �64   6A   .+�5v      o� 
������ $��2��6,   $��0 0 0 10q�6@   .@�5w      m� 
������ 
������ ��5x      �� 5y      $ 
Horizontal=�6%   .+�5z      �� $	����6(   $
��Bold"�68   $9��3pack://application:,,,/Resources/#Segoe MDL2 Assets)�6J   $��20(�6�   �� 5{      -   5        $ 
tblCalType��5{   (     setting_upper#���  6<   $��Tahoma)�6c   $��
ExtraLight"�6w   $��13(�6�   $��Bottom=�6�   $��5 0 0 0q�6�   5|      .@�5      m� -   5          m�Name$  mnuiStandard��5      ! m� Icon! 5�      �� 5�      -   5        $ tblstandardIcon��5�   (   $9��3pack://application:,,,/Resources/#Segoe MDL2 Assets)�6A   $	����6�   ��5�      �� 5�      $��Tahoma)�6(   $��14(�6<   $��0 10 0 0q�6J     standard#���  6\   5�      m� 5�      -   5        $  
mnuiExtend��5�      ! 5�      �� 5�      -   5        $ tblExtendIcon��5�   (   $9��3pack://application:,,,/Resources/#Segoe MDL2 Assets)�6?   $	����6�   ��5�      �� 5�      $��Tahoma)�6(   $��14(�6<   $��0 10 0 0q�6J    
	 extend#��� 	 6\   5�      5�      5�      � 5�      -   5        $ grdPayTemplate��5�      $��1��6+   $��0,0,0,0q�68    5�      �� 5�      6&   �� 5�      6&   �� 5�      6&   �� 5�      6&   �� 5�      6&    5�      �� 5�      6)   �� 5�      6)   �� 5�      6)   �� 5�      6)   5�      � 5�      -   5        $ 	grdPayNum��5�      $��1��6&   $��1��66   $��0 0 0 0q�6C   " +� IsFocusScope" .  5�      �� 5�      6&   �� 5�      6&   �� 5�      6&   �� 5�      6&   �� 5�      6&    5�      �� 5�      6)   �� 5�      6)   �� 5�      6)   .+�5�      �� -   5        # ��Name$# contentTotal��5�      $��0 0 2 2q�6/   $��0��6�   $��0��6�   $��2��6�    
 buttonMoneyStyle#��[
 6�   $��15(�6  ��6@   �� *DataContext.GetTotalTxt+$ TwoWay=�$ PropertyChanged=� �� *Self+��6-  �� *getTotalCmd+��6M  �� *	Content+$ contentTotal���� 5�      -   5        $# btnBackspace��5�      $��0 0 0 2q�6J     	backspace#���  6[   $��0��6�   $��2��6�   #��[
 6�   $��15(�6�   �� 5�      -   5        $ 7��5�      $��0 0 2 2q�66   $��7��6G   $��1��6S   $��0��6`   #��[
 6p   $��15(�6�   �� 5�      -   5        $ 8��5�      $��0 0 2 2q�66   $��8��6G   $��1��6S   $��1��6`   #��[
 6p   $��15(�6�   �� 5�      -   5        $ 9��5�      $��0 0 0 2q�66   $��9��6G   $��1��6S   $��2��6`   #��[
 6p   $��15(�6�   �� 5�      -   5        $ 4��5�      $��0 0 2 2q�66   $��4��6G   $��2��6S   $��0��6`   #��[
 6p   $��15(�6�   �� 5�      -   5        $ 5��5�      $��0 0 2 2q�66   $��5��6G   $��2��6S   $��1��6`   #��[
 6p   $��15(�6�   �� 5�      -   5        $ 6��5�      $��0 0 0 2q�66   $��6��6G   $��2��6S   $��2��6`   #��[
 6p   $��15(�6�   �� 5�      -   5        $ 1��5�      $��0 0 2 2q�66   $��1��6G   $��3��6S   $��0��6`   #��[
 6p   $��15(�6�   �� 5�      -   5        $ 2��5�      $��0 0 2 2q�66   $��2��6G   $��3��6S   $��1��6`   #��[
 6p   $��15(�6�   �� 5�      -   5        $ 3��5�      $��0 0 0 2q�66   $��3��6G   $��3��6S   $��2��6`   #��[
 6p   $��15(�6�   �� 5�      -   5        $ 0��5�      $��0 0 2 2q�66   $��0��6G   $��4��6S   $��0��6`   #��[
 6p   $��15(�6�   �� 5�      -   5        $# btnpoint��5�      $��0 0 2 2q�6B   $��.��6S   $��4��6_   $��1��6l   #��[
 6|   $��15(�6�   �� 5�      -   5        $# btnClear��5�      $��0 0 0 2q�6B   $��C��6S   $��4��6_   $��2��6l   #��[
 6|   $��15(�6�   5�      5�      �� 5�   
   $ 
Horizontal=�6   $��Center=�6.   $��Bottom=�6K   $��2��6g   $��0,50,0,0q�6{   $��1��6�   .+�5�      � .+�5�      �� -    5        $# btnSaveInvoice��5�   p   $	��120��6�     save_invoice#���  6�   {�6   �� *IsActiveButton+$ TwoWay=�$ PropertyChanged=�6�   =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  5�      -!   5        $  Name$$ mprSaveInvoice��5�   )   %   IsActive% .  $y�Hidden=�6R      ThreeBounceProgressRingStyle#��[ 6f   6�   5�      � 5�      .+�5�      �� -"   5        $# btnSaveSendEmail��5�   p     save_print_or_send_email#���  6�   $��5,0,0,0q�6�   {�6   �� *IsActiveButton+$ TwoWay=�$ PropertyChanged=�6�     5�      -#   5        $$ mprSaveSendEmail��5�   )   % .  $y�Hidden=�6T   #��[ 6h   6�   5�      �� 5�      -$   5        $# btnClose��5�   ,   $��5,0,0,0q�6>   $	��100��6O    	 close#���  6[   6�   5�      5�      5�      