   M S B A M L   `   `   `  ����H  DCash Register, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:CashierRegister.ViewModelCashierRegister.ViewModel  5      A%clr-namespace:CashierRegister.HelpersCashierRegister.Helpers  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5	      '    !CashierRegister.Views.PaymentScan   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      7	viewModel'clr-namespace:CashierRegister.ViewModel  5      3Helpers%clr-namespace:CashierRegister.Helpers  5      XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5	         giftcard_info  � Title#  �   5       � DataContext 5      4   .CashierRegister.ViewModel.PaymentScanViewModel  5   
   6+   .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *scanGiftLoadedCmd+6L   5      .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5!   
   �� 5"      $	��100 �6   6-   �� 5#      6!   .+�5%   
   �� $��0��6   
���  ��$��2��6�   $��0,0,0,10q�6�   ��6!   �� *StringNotify+ �� Mode$ TwoWay=�	 �� UpdateSourceTrigger$	 PropertyChanged=�6�   �� 5'   
     barcode#���  6   $��1��65   $��0��6B   $��0,2,0,5q�6R   6f   �� 5(   
   -   5        
 ��Name$
 
txtBarcode��5(   j   $��1��6]   $��1��6~   $��1��6�   $��25��6�   $��Center=�6�   $��Center=�6�   $��0,0,0,5q�6�   1   +CashierRegister.Helpers.TextBoxMaskBehavior   Mask$ Integer=�6�      MinimumValue$ 0Q�6      MaximumValue$ 9999999999999Q�6M  -   'CashierRegister.Helpers.SetControlFocus   	IsFocused$
 TrueX�6�  ��6   �� *	Barcode+$ TwoWay=�$	 PropertyChanged=�.��6�   J� InputBindings 5)      �� 5*      
 �� Key$ Return��6    �� Command 6*   �� *keyDownEnterCmd+ �� CommandParameter 6N   �� *Text+ �� ElementName$ 
txtBarcode��6�   .��5+   %    5,        5-      $ 	LostFocus��6!   . 5.         6,   �� *lostFocusCmd+6O   5/      .��50   &    � ToolTip 51      �� 52        press_giftcard_enter#���  6   6L   .��53      54      �� 56   
     giftcard_balance#���  6   $��2��6>   $��0,7,0,5q�6K   6^   �� 57   
   $��2��6a    �� 
IsReadOnly . $��1��6�   $��25��6�   $��Center=�6�   $��Center=�6�   $��0,5,0,5q�6�   ��6   �� *GiftBalance+$ TwoWay=�$	 PropertyChanged=�6�   �� 59   
     serial_number#���  6   $��3��6;   $��0,7,0,5q�6H   6[   �� 5:   
   $��3��6`   $��1��6m   $��25��6}   $��Center=�6�   $��Center=�6�    . $��0,5,0,5q�6�   ��6   �� *
GiftSerial+$ TwoWay=�$	 PropertyChanged=�6�   �� 5<   
   $	��Use��6   $��4��6   $��0,7,0,5q�6,   6?   �� 5=   
   -   5        $
 txtUseValue��5=   k   $��2��6^   $��4��6�   $��1��6�   $��25��6�   $��Center=�6�   $��Center=�6�   $��0,5,0,5q�6�   $ Decimal=�6�   $ 999999999.99Q�6"  $ 0Q�6Z  ��6   �� *
UseValue+$ TwoWay=�$	 PropertyChanged=�6�  �� 5?   
    �� Orientation$ 
Horizontal=�6   $��5��6.   $��2��6;   $��Right=�6O   $	��Top=�6k   $��0, 40, 0, 5q�6�   .+�5@      �� -   5         ��Name$ muiBtnOK��5@      $��3��6O   $��80��6\     ok#���  6g   $��0,0,5,0q�6�   ��6'   �� *starScanGiftCadrCmd+6�   �� 5A      -   5        $ muiBtnCancel��5A      $��80��6+    
 cancel#���  66   $��4��6�   ��6Y   �� *CloseWindowCommand+$ OneWay=�6�   5B      �� 5C   
     press_0_clear#���  6   $��2��6;   $��6��6O   $��0,5,0,0q�6\   6o   5D      5F      