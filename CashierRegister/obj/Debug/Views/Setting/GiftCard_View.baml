   M S B A M L   `   `   `  ����H  DCash Register, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:CashierRegister.ViewModelCashierRegister.ViewModel  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5      A%clr-namespace:CashierRegister.HelpersCashierRegister.Helpers  5	      1    +CashierRegister.Views.Setting.GiftCard_View   5      U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.8.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      7	viewModel'clr-namespace:CashierRegister.ViewModel  5      XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5      2Helper%clr-namespace:CashierRegister.Helpers  5	         
send_email  � Title#  �   5       � DataContext 5      5   /CashierRegister.ViewModel.GiftCard_AddViewModel  5   
   .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *formLoadCmd+6F   5      .��5      � $��Center=�6    � RowDefinitions 5   
   �� 5      $��35 �6   6*   �� 5      $��35 �6   6*   �� 5      $��35 �6   6*   �� 5      $��35 �6   6*   �� 5      $��* �6   6)   �� 5      $��35 �6   6*   �� 5      $��35 �6   6*   �� 5       $��50 �6   6*    � ColumnDefinitions 5"   
   �� 5#      $
��Auto �6   6.   �� 5$      $��* �6   6+   �� 5%      $
��Auto �6   6.   .+�5(   
   ��   from#���  6   $��SemiBold"�62   $��Center=�6H   $��	0 0 25 10q�6c   6x   �� 5)   
   -   5         ��Name$ txbFrom��5)      $��25��6p   $��Center=�6|   $��1��6�   $��Center=�6�   $��0 0 0 10q�6�   ��6#   �� *	FromEmail+	 �� Mode$	 TwoWay=�
 �� UpdateSourceTrigger$
 PropertyChanged=�6�   �� 5*   
   -   5         ��Name$ imgFromSave��5*      $��Center=�6%   $��5 0 0 10q�6@   $��24��6R   $��24��6]   $��2��6i    � ToolTip 5+      �� 5,        
save_email#���  6   6B   5.      �� 50   
     password#���  6   $��SemiBold"�66   $��Center=�6L   $��1��6g   $��	0 0 25 10q�6t   6�   G� 51   
   -   5         G�Name$ pwbPasssword��51      $��25��6,   $��Center=�68   $��1��6Z   $��1��6j   $��0 0 0 10q�6w   ,   &CashierRegister.Helpers.PasswordHelper   Attach .    Password 6�   �� *FromPassword+$	 TwoWay=�$
 PropertyChanged=�6  �� 52   
   -   5        $ imgPasswordSave��52      $��Center=�6)   $��1��6D   $��5 0 0 10q�6Q   $��24��6c   $��24��6n   $��2��6z    53      �� 54        save_password#���  6   6E   56      �� 58   
     to#���  6   $��SemiBold"�60   $��Center=�6F   $��2��6a   $��0 0 0 10q�6n   6�   �� 59   
   -   5        $ txbTo��59      $��1��6l   $��25��6|   $��Center=�6�   $��2��6�   $��Center=�6�   $��0 0 0 10q�6�   $��2��6�   ��6!   �� *	ToEmail+$	 TwoWay=�$
 PropertyChanged=�6�   �� 5;   
   $��3��6   $��SemiBold"�6!     subject#���  67   $��Center=�6X   $��0 0 0 5q�6s   6�   �� 5<   
   -   5        $ 
txbSubject��5<      $��3��6v   $��25��6�   $��Center=�6�   $��1��6�   $��2��6�   $��Center=�6�   $��0,0,0,10q�6�   ��6&   �� *SubjectEmail+$	 TwoWay=�$
 PropertyChanged=�6  �� 5>   
   -   5         ��Name$ rtbMailBody��5>      $��0 0 0 0q�6+   $	��200��6<    �� AcceptsReturn . $��4��6^   $��3��6k   /   )CashierRegister.Helpers.RichTextBoxHelper   DocumentXaml 6   �� *	BodyEmail+$	 TwoWay=�$
 PropertyChanged=� � 	Resources 5?       �� 
TargetType �� Property �� Value��%]   'J�       �� 5@      " J�6   .�5A      ��  �@��Margin$ 0q�6/   6;   5B      5C      5D      �� 5F   
     giftcard_sent_deliver#���  6   $��5��6E   $��3��6R   $��0 5 0 0q�6�    d� 	IsChecked 6f   �� *IsTick+$	 TwoWay=�$
 PropertyChanged=�6�   �� 5G   
   $��6��6   $��3��6!   $��
0, 5, 0, 0q�6   
���  ����65   �� *Notify+$	 OneWay=�$
 PropertyChanged=�6�   �� 5I   
    �� Orientation$ 
Horizontal=�6   $��7��6.   $��3��6;   $��Right=�6O   $��Bottom=�6k   $��
0, 0, 0, 5q�6�   .+�5J      �� -   5         ��Name$ muiBtnOK��5J      $��80��6M     single_send#���  6X   $��0,0,5,0q�6�   ��6'   �� *sendGiftCardToCmd+y�6�   �� *	IsVisible+$	 TwoWay=�$
 PropertyChanged=�6�   �� 5K      -	   5        $ muiBtnCancel��5K      $��80��6+    
	 cancel#��� 	 66   ��6Y   �� *sendEmailCloseWindowsCmd+$	 OneWay=�y�6�   �� *	IsVisible+$	 TwoWay=�$
 PropertyChanged=�6�   =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  5L      -
   5          Name$ mprSend��5L   %   $��25��66   $��25��6A      IsActive .  !
 DoubleBounceProgressRingStyle#��[
 6�   y�6]   �� *EnableProgress+$	 TwoWay=�$
 PropertyChanged=�6�   5M      5N      5P      