   M S B A M L   `   `   `  ����H  DCash Register, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:CashierRegister.ViewModelCashierRegister.ViewModel  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5	      A%clr-namespace:CashierRegister.HelpersCashierRegister.Helpers  5
      0    *CashierRegister.Views.Setting.GiftCard_Add   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      7	viewModel'clr-namespace:CashierRegister.ViewModel  5      XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5	      3Helpers%clr-namespace:CashierRegister.Helpers  5
      -   5        7  1FirstFloor.ModernUI.Windows.Controls.ModernDialog   Name$  addGiftCardWindows��5         add_gift_card � Title# �   5       � DataContext 5      5   /CashierRegister.ViewModel.GiftCard_AddViewModel  5   
    J� InputBindings 5      �� 5   
   
 �� Key$ Return��6    �� Command 6"   �� *formAddEnterKeyDownCmd+ �� CommandParameter 6M   �� *Giftcard_Info+6z    � 	Resources 5      9   3CashierRegister.Helpers.EnumMatchToBooleanConverter  FilterExpire��%   &	         5   
   6E   5      .��5      � $��Center=�6   $	��350��6&    � ColumnDefinitions 5   
   �� 5      $	��150 �6   6-   �� 5      $��* �6   6+   	 � RowDefinitions	 5   
   �� 5      6   �� 5       6   �� 5!      6   �� 5"      6   �� 5#      6   �� 5$      6   �� 5%      6   .+�5(   
   �� $��Center=�6,   $��0,0,0,15q�6G   
���""��$��2��6p   ��6   �� *Notify+6�   �� 5*   
     num_of_giftcard#���  6   $��Center=�6=   $��Medium"�6X   $��0,0,0,10q�6l   $��1��6~   6�   �� 5+   
   -   5        
 ��Name$
 NumofTexbox��5+   c   $��1��6x   $��1��6�   $��25��6�   $��Center=�6�   $��Center=�6�   $��0,0,0,10q�6�   1   +CashierRegister.Helpers.TextBoxMaskBehavior   Mask$ Integer=�6�      MinimumValue$ 0Q�6     MaximumValue$	 999Q�6H  -   'CashierRegister.Helpers.SetControlFocus   	IsFocused$
 TrueX�6w  ��6   �� *NumOfGiftCard+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.��6�  5,      �� 5.   
    
 amount#���  6   $��Center=�64   $��Medium"�6O   $��0,0,0,10q�6c   $��2��6u   6�   �� 5/   
   -   5        $
 AmountTexbox��5/   b   $��1��6x   $	��100��6�   $
��Left=�6�   $��2��6�   $��25��6�   $��Center=�6�   $��Center=�6�   $��0,0,0,10q�6  $ Decimal=�6  $ 0Q�6B  $ 999999999.99Q�6o  ��6   �� *StringAmount+$ TwoWay=�$ PropertyChanged=�6�  �� 50   
   $��2��6>   $��1��6K   $��	110,4,0,0q�6[   ��6   �� *ShowCurency+$ OneWay=�6p   �� 52   
     expiration_days#���  6   $��Center=�6=   $��Medium"�6X   $��0,0,0,10q�6l   $��3��6~   $y�	Collapsed=�6�   6�   �� 53   
   $	��Top=�6   $��1��6+   $��3��6;   $��0,0,0,10q�6H    �� SelectedIndex$ 0��6Z   $y�	Collapsed=�6l   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6�    �� SelectedItem 6�   �� *SelectedExpiration+$ TwoWay=�$ PropertyChanged=�54      	� 56   
   $��4��6   $��Center=�6#   $��0,0,0,10q�6>    d� 	IsChecked 6P   �� *CurrentOption+ �� 	Converter# [ *   $CashierRegister.ViewModel.ExprieDate   AfterDay �� ConverterParameter# Z .��57      ��   expires_after#���  6   6A   58      �� 59   
   -   5        $
 txtDays��59   h   $��4��6   $��1��6   $��0,0,0,10q�6/   $	��100��6A   $
��Left=�6M   $ Integer=�6�   $ 0Q�6  $	 999Q�6A  {�6y   �� *IsEnableDay+��6�   �� *
ExpireDays+$ TwoWay=�$ PropertyChanged=�6r  �� 5:   
    	 day_s#���  6   $��4��63   $��1��6@   $��	110,2,0,0q�6P   6e   	� 5<   
   $��5��6   $��Center=�6#   $��0,0,0,10q�6>    6P   �� *CurrentOption+# [    ToDate# Z .��5=      ��   
expires_on#���  6   6>   5>      (  "System.Windows.Controls.DatePicker  5?   
   -   5          Name$ dpDate��5?   "   $��5��6   $��1��62   $��0,0,0,10q�6B      Text 6T   �� *ExpirationDate+$ TwoWay=�$ PropertyChanged=�{�6�   �� *IsEnableToDate+6�   �� 5A   
    �� Orientation$ 
Horizontal=�6   $��6��6.   $��1��6;   $��Right=�6K   $	��Top=�6g   $��0, 40, 0, 0q�6   .+�5B      �� -   5         ��Name$ muiBtnOK��5B      $��80��6t     ok#���  6   $��0,0,5,0q�6�   ��6'   �� *okGiftCardCmd+��6I   �� *Giftcard_Info+y�6�   �� *	IsVisible+$ TwoWay=�$ PropertyChanged=�6  �� 5C      -   5        $ muiBtnCancel��5C      $��80��6+    
	 cancel#��� 	 66   ��6Y   �� *CloseWindowCommand+$ OneWay=���6�   ��  �� ElementName$ addGiftCardWindows��y�6�   �� *	IsVisible+$ TwoWay=�$ PropertyChanged=�6  =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  5D      -   5          Name$ mprSend��5D   %   $��25��66   $��25��6A      IsActive .  !
 DoubleBounceProgressRingStyle#��[
 6�   y�6]   �� *EnableProgress+$ TwoWay=�$ PropertyChanged=�6�   5E      5G      5I      