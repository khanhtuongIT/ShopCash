   M S B A M L   `   `   `  ����H  DCash Register, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:CashierRegister.ViewModelCashierRegister.ViewModel  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5	      A%clr-namespace:CashierRegister.HelpersCashierRegister.Helpers  5
      1    +CashierRegister.Views.Setting.GiftCard_Edit   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      7	viewModel'clr-namespace:CashierRegister.ViewModel  5      XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5	      3Helpers%clr-namespace:CashierRegister.Helpers  5
      -   5        7  1FirstFloor.ModernUI.Windows.Controls.ModernDialog   Name$  editGiftCardWindows��5         edit_giftcard � Title# �   5       � DataContext 5      5   /CashierRegister.ViewModel.GiftCard_AddViewModel  5   
   .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *formLoadCmd+6F   5       J� InputBindings 5      �� 5   
   
 �� Key$ Return��6   	 �� Command	 6"   �� *formEditEnterKeyDownCmd+
 �� CommandParameter
 6N   �� *Text+ �� ElementName$ AmountTexbox��6�    � 	Resources 5      9   3CashierRegister.Helpers.EnumMatchToBooleanConverter  FilterExpire��%   &	         5   
   6E   5      .��5      � $��Center=�6   $	��350��6&    � ColumnDefinitions 5   
   �� 5       $	��150 �6   6-   �� 5!      $��* �6   6+    � RowDefinitions 5#   
   �� 5$      6   �� 5%      6   �� 5&      6   �� 5'      6   �� 5(      6   �� 5)      6   �� 5*      6   �� 5+      6   �� 5,      6   .+�5/   
   ��   giftcard_barcode#���  6   $��Center=�6>   $��Medium"�6Y   $��0,0,0,10q�6m   6�   �� 50   
   $��1��6H   ��6   �� *Giftcard_Info.Barcode+ �� Mode$ OneWay=�6Z   �� 52   
     giftcard_serial#���  6   $��Center=�6=   $��Medium"�6X   $��0,0,0,10q�6l   $��1��6~   6�   �� 53   
   -   5         ��Name$ NumofTexbox��53   d   $��1��6G   $��1��6W   ��6   �� *Giftcard_Info.Serial+$ OneWay=�6{   �� 55   
     created#���  6   $��Center=�65   $��Medium"�6P   $��0,0,0,10q�6d   $��2��6v   6�   �� 56   
   -   5        $	 tbt��56   h   $��1��6K   $��2��6[   ��6   �� *Giftcard_Info.CreateDate+$ OneWay=�6w   �� 58   
    
 amount#���  6   $��Center=�64   $��Medium"�6O   $��0,0,0,10q�6c   $��3��6u   6�   �� 59   
   -   5         ��Name$ AmountTexbox��59   0   $��1��6F   $	��100��6V   $
��Left=�6b   $��3��6}   $��25��6�   $��Center=�6�   $��Center=�6�   $��0,0,0,10q�6�   1   +CashierRegister.Helpers.TextBoxMaskBehavior   Mask$ Decimal=�6�      MinimumValue$ 0Q�6     MaximumValue$ 999999999.99Q�6=  ��6   �� *StringAmount+6w  �� 5:   
   $��3��6>   $��1��6K   $��	110,4,0,0q�6[   ��6   �� *ShowCurency+$ OneWay=�6p   	� 5<   
   $��4��6   $��Center=�6#   $��0,0,0,10q�6>    d� 	IsChecked 6P   �� *CurrentOption+ �� 	Converter# [  �� ConverterParameter$ AfterDay��.��5=      ��   expires_after#���  6   6A   5>      �� 5?   
   -   5        $ txtDays��5?   h   $��4��6   $��1��6   $��0,0,0,10q�6/   $	��100��6A   $
��Left=�6M   $ Integer=�6�   $ 0Q�6  $	 999Q�6A  {�6y   �� *IsEnableDay+��6�   �� *
ExpireDays+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6r  �� 5@   
    	 day_s#���  6   $��4��63   $��1��6@   $��	110,2,0,0q�6P   6e   	� 5B   
   $��5��6   $��Center=�6#   $��0,0,0,10q�6>    6P   �� *CurrentOption+# [ $ ToDate��.��5C      ��   
expires_on#���  6   6>   5D      (	  "System.Windows.Controls.DatePicker	  5E   
   -   5         	 Name$ dpDate��5E   "   $��5��6   $��1��62   $��0,0,0,10q�6B    	  Text 6T   �� *ExpirationDate+$ TwoWay=�$ PropertyChanged=�{�6�   �� *IsEnableToDate+6�   �� 5G   
    	 giftcard_sent_deliver#��� 	 6   $��6��6E   $��2��6R    6f   �� *
IsEditTick+$ TwoWay=�$ PropertyChanged=�6�   �� 5P   
   $��Center=�6,   $��0,0,0,15q�6G   
���""��$��7��6p   $��2��6}   ��6   �� *Notify+6�   �� 5R   
    �� Orientation$ 
Horizontal=�6   $��8��6.   $��1��6;   $��Right=�6K   $	��Top=�6g   $��0, 20, 0, 0q�6   .+�5S      �� -   5         ��Name$ muiBtnOK��5S      $��80��6�    
 ok#��� 
 6�   $��0,0,5,0q�6�   ��6'   �� *okSaveGiftCardCmd+$ OneWay=���6Z   �� *Text+$ AmountTexbox��6�   �� 5T      -   5        $ muiBtnCancel��5T      $��80��6+    
 cancel#���  66   ��6Y   �� *CloseWindowCommand+$ OneWay=���6�   �� $ editGiftCardWindows��6�   5U      5W      5Y      