   M S B A M L   `   `   `  ���� C  ?terminal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Universe.Terminal.UIUniverse.Terminal.UI  #    Universe.Terminal.UI.Terminal   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:Universe.Terminal.UI     -     �Name$  window��$��384.114��$��671.562�� � AllowsTransparency .  � WindowStyle$
 None=� � Topmost .  � ShowInTaskbar .   � 
ResizeMode$ NoResize=�
������  � WindowStartupLocation$ CenterScreen=�$��Center=�$��Center=�$��Center=�$��Center=� � SizeToContent$ WidthAndHeight=�.��� -    �Name$ Frame��	 J� RenderTransformOrigin$		 0,1&�$��2q�.+��� -   
 ��Name$
 border�� �� Padding$ 4q�$��4q����	#FF2C3E50 �� CornerRadius$ 10��$��1q�$��2��$	��350��$	��450�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     Color$ 	#FF25313E��   Opacity$
 0.35Q�   
BlurRadius$ 3Q�._��� -   $��Scroller��$��0q� � UseLayoutRounding . ���� *Background.Color+ �� ElementName$ border��.���� -   $��consoleItems��.+��� -    ��Name$ inputSection�� �� Orientation$ 
Horizontal=�.+��� -    ��Name$ 	inputSign��$��5, 10, 1, 1q�$��20(� �� 
IsReadOnly .  �� IsReadOnlyCaretVisible . ���	#FFECF0F1 � IsSharedSizeScope . $��SemiBold"�$��Segoe UI Semibold)�   Triggers#���   ���� $ border�� �� Mode$ OneWay=���Z�  �� SelectionBrush Z� .��>�� -   $��
InputBlock��$��18(�$��0, 10, 10, 1q�$�� ��
�������$��Segoe UI Semibold)� �� TextWrapping$
 Wrap=� �	#FFECF0F1#���   ���� *Background.Color+$ border����Z� .�� �� 
CaretBrush ��  �� Color$ 	#FFECF0F1��.������  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,1&�.���   � Color$  	#FFE74C3C��! � Offset$	! 0.0Q�� $  	#FFE67E22��$
! 0.17Q�� $  	#FFF1C40F��$
! 0.33Q�� $  	#FF2ECC71��$	! 0.5Q�� $  	#FF3498DB��$
! 0.67Q�� $  	#FF8E44AD��$
! 0.83Q�� $  	#FF9B59B6��$	! 1.0Q�" � Triggers" :� # :� RoutedEvent$# Window.Loaded��.[��� .���� .��W� $ �� TargetProperty$$ Opacity�% W� From$	% 0.0X�	& W� To$	& 1.0X�' g� Duration$' 0:0:1.5D�:� $# Window.Unloaded��.[��� .���� .��W� $$ Opacity�$	% 1.0X�$	& 0.0X�$' 0:0:1D�( � 	Resources(   RoundedButtonStyle) �� 
TargetType* �� Property+ �� Value, �� 
TargetType  SwitchControl- � 	Resources  Key. g� 	BeginTime/ �� 
TargetName0 O� KeyTime1 O� Value	2 �� To3 p� KeyTime4 p� Value5 ]�Name6 J� SnapsToDevicePixels7 �� ContentStringFormat8 �� ContentStringFormat9 K� ZIndex: �� ScaleX; �� ScaleY< �� AngleX= �� AngleY> �� Angle? T� X@ T� YA �� TriggersB P� PropertyC P� ValueD N� ExitActionsE ��Name  OnUncheckingF N� EnterActions  
OnCheckingG ��NameH �� 
TargetName  StandardBorderBrush  StandardBackgroundBrush  HoverBorderBrush 	 SelectedBackgroundBrush 
 SelectedForegroundBrush  DisabledForegroundBrush  NormalBrush  NormalBorderBrush  HorizontalNormalBrush  HorizontalNormalBorderBrush  ListBoxBackgroundBrush  StandardBrush  
GlyphBrush  PressedBrush  ScrollBarLineButtonI �� PathJ �� RelativeSource  ScrollBarPageButton  ScrollBarThumb  VerticalScrollBarK � RowDefinitionsL ]� DecreaseRepeatButtonM ]� ThumbN ]� IncreaseRepeatButton  HorizontalScrollBarO � ColumnDefinitionsP �� Triggers��%�  &	       &	 {     7  7  &	      &	 �    &	 �    &	 �    &		 �    &	
 �    &	 �    &	 �    &	 �    &	     &	 %    &	 :    &	 �    &	 		    &	 	    &	 �	    7  7  7  7  7  &	 +    &	 �    &	 �    7  7  7  7  7  7  7  &	 r    7  7  7  7  7  7  7  '�� .    7  7  �� ") ��.��� * � ��+ �� ", ��.R��� $ 5�����	#FF9B59B6$��1q�$ 2q�._��� $��Center=�$��Center=��� ") ��.��� * � ��
+ ������� * � ��+ �	#FF2C3E50�� * � ��$	+ Off���� * � ��+ �� ", ��- ���� 
OnChecking .��U� $. 00:00:00X�$/ slider��$U$ O(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)�.\��� $0 00:00:00.3000000��$1 33Q��� $2 	#FF3498DBX�$/ 
switchBack��$1$ +(Border.Background).(SolidColorBrush.Color)�$' 0:0:1D��� OnUnchecking .��U� $. 00:00:00X�$/ slider��$U$ O(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)�.\��� $0 00:00:00.3000000��$1 0Q�r� $. 00:00:00X�$/ slider��$$ (FrameworkElement.Margin)�.	��� $3 00:00:00.3000000��$4 1,1,1,1q��� $2 	#FF2C3E50X�$/ 
switchBack��$1$ +(Border.Background).(SolidColorBrush.Color)�$' 0:0:1D�.R�]� $5 	dockPanel��.+��� $��0,0,5,0q�$��Right=�$��Center=�#6 z6 #��z��#8 z7 #��z����. �� $
 
switchBack��$9 5��$ 15��$��1q����	#FF2C3E50$��60��$��28��
���   �._�� $��65��
������ .+��� $
 slider��$
��Left=�$��1q�$��22��$��0q����	#FF1E252D$	 0.5,0.5&�$ 25��$��22��z�V� .~��� $: 1Q�$; 1Q��� $< 0Q�$= 0Q��� $> 0Q�T� $? 0Q�$@ 0Q�����	White��A P� B �@d�	IsChecked$
C TrueY�D �� $"E OnUnchecking_BeginStoryboard��8��  F �� $ E OnChecking_BeginStoryboard��8�� . ��� * � ��$+ On��P� B � {�C .  . ��� #+ � ��* � ���� ") ��.��� * � ��+ �� ", ��.R��� $
 border��#��z��#��z��#��z��6 . ._��� $G PART_ContentHost��|�.  $��Hidden=�$��Hidden=�A P� B � {�C .  . ��� $H border��* � ��
+ ���� P� B �@J�IsMouseOverC . . ��� $H border��* � ��
+ ���� P� B �@J�	IsFocusedC . . ��� $H border��* � ��
+ ���� �� $
 #888���� $
 #FFF���� $
 #DDD���� $
 Gray���� $ White���� $
 #888���� $ 	#FF3498DB���� $ 	#FF3498DB���� $ 	#FF3498DB���� $ 	#FF3498DB���� $	 0,0&�$ 1,0.001&���� � $  White��$	! 0.0Q�� $  White��$	! 0.6Q�� $  #DDDDDD��$	! 1.2Q��� $	 0,0&�$	 0,1&���� � $
  #FFF��$	! 0.0Q�� $
  #CCC��$	! 1.0Q��� $ 	#FF2C3E50���� $	 0,0&�$	 0,1&���� � $
  #BBB��$	! 0.0Q�� $
  #EEE��$	! 0.1Q�� $
  #EEE��$	! 0.9Q�� $
  #FFF��$	! 1.0Q��� ") ��.��� * �@J�SnapsToDevicePixels+ . �� * �@�OverridesDefaultStyle+ . �� * � |�+ .  �� * � ��+ �� ", ��.R��� $��Border��$��1q�$ 2��8��  8�� $��1q�._�F� $��Center=�$��Center=�8�� ���� $I Content�J �� *TemplatedParent+A P� B � ��C . . ��� $H Border��8+  * � ��P� B � {�C .  . ��� 8+  * � ���� ") ��.��� * �@J�SnapsToDevicePixels+ . �� * �@�OverridesDefaultStyle+ . �� * � ��+ .  �� * � |�+ .  �� * � ��+ �� ", ��.R��� 
������ �� ") n�.��� * �@J�SnapsToDevicePixels+ . �� * �@�OverridesDefaultStyle+ . �� * � ��+ .  �� * � |�+ .  �� * � ��+ �� ", n�.R��� $ 2��$��1q����	#FF3498DB���	#FF3498DB�� ", ��.R�� K �� $��18���� $��0.00001* ��� $��18��.+��� $��3��$ 2��
������ �� $��0��8��  $��18��$��ScrollBar.LineUpCommand��$��M 0 4 L 8 4 L 4 0 Z��]� $��
PART_Track��$��1����. L �� 8�� $��ScrollBar.PageUpCommand��M n� 8�� $��1,0,1,0q�8�� 8�� N �� 8�� $��ScrollBar.PageDownCommand���� $��3��8�� $��18��$��ScrollBar.LineDownCommand��$��M 0 0 L 4 4 L 8 0 Z���� ", ��.R�� O �� $��18���� $��0.00001* ��� $��18��.+��� $��3��$ 2��
������ �� $��0��8��  $��18��$��ScrollBar.LineLeftCommand��$��M 4 0 L 4 8 L 0 4 Z��]� $��
PART_Track��$��1����.  L �� 8�� $��ScrollBar.PageLeftCommand��M n� 8�� $��0,1,0,1q�8�� 8�� N �� 8�� $ ��ScrollBar.PageRightCommand���� $��3��8�� $��18��$ ��ScrollBar.LineRightCommand��$��M 0 0 L 4 4 L 0 8 Z���� ") ��.��� * �@J�SnapsToDevicePixels+ . �� * �@�OverridesDefaultStyle+ . P P� B �@��Orientation$C 
Horizontal=�. ��� * � ��$
+ Auto���� * � ��$+ 18���� * � ��8+   P� B �@��Orientation$C Vertical=�. ��� * � ��$+ 18���� * � ��$
+ Auto���� * � ��8+  