   M S B A M L   `   `   `  ����K  GLocal Haberlesme, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:LocalHaberlesmeLocalHaberlesme  5      #    LocalHaberlesme.SunucuOlustur   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      U QMaterialDesignThemes.Wpf, Version=3.0.1.920, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      )localclr-namespace:LocalHaberlesme  5        N� Title$  SunucuOlustur��5
       N� ShowsNavigationUI .  $	��718��6M   $	��391��6Y    N� 
Foreground 67   Z� .��5      � .+�5   
   �� $��1q�6   $��10q�6%    �� CornerRadius$ 40��61   ���	#FF222324._�5      � .+�5      �� -   5         ��Name$ kullaniciAdiTb��5      $
��Left=�62   $��31��6M   $��314,107,0,0q�6Y    �� TextWrapping$
 Wrap=�6n   $�� ��6�   $	��Top=�6�   $	��206��6�   $��18(�6�    �� TextAlignment$ Center=�6�   
����������	#FF36393F �� 	MaxLength$ 12��6  ��6�   Z� 6  �� 5      -   5        $ 	ipAdresTb��5      $
��Left=�6-   $��31��6H   $��314,169,0,0q�6T   $
 Wrap=�6i   $��000.000.000.000��6}   $	��Top=�6�   $	��206��6�   $��18(�6�   $ Center=�6�   
����������	#FF36393F$ 15��6   �� 
IsReadOnly . ��6  Z� 6-  �� 5      -   5        $ portTb��5      $
��Left=�6*   $��31��6E   $��314,231,0,0q�6Q   $
 Wrap=�6f   $�� ��6z   $	��Top=�6�   $	��206��6�   $��18(�6�   $ Center=�6�   
����������	#FF36393F$ 5��6  ��6�   Z� 6  �� 5      -   5        	 ��Name$		 lbl��5      $��Sohbet Oluştur��6%   $��
220,10,0,0q�6>   $	��Top=�6R   ���	#FFFF9800$
��Bold"�6�   $��36(�6�   $��58��6�   $
��Left=�6�   ��6j   Z� 6�   �� 5      $��
IP Adresi:��6   $
��Left=�6-   $��206,165,0,0q�6H   $	��Top=�6]   ���	#FFFF9800$
��Bold"�6�   $��22(�6�   $��39��6�   $	��108��6�   �� 5      $��PORT:��6   $
��Left=�6(   $��242,228,0,0q�6C   $	��Top=�6X   ���	#FFFF9800$
��Bold"�6�   $��22(�6�   $��39��6�   $��72��6�   �� 5      -   5        
 ��Name$
 
olusturBtn��5         MaterialDesignRaisedButton#���   6-   $��Sohbet Oluştur��6b   $��256,293,0,0q�6{   ���	#FF36393F���	#FF222324���	#FFFF9800$
��Bold"�6�   $��33��6�   $	��Top=�6�   $
��Left=�6  $	��206��6'  63  �� 5      $��Kullanıcı Adı:��6   $
��Left=�61   $��170,104,0,0q�6L   $	��Top=�6a   ���	#FFFF9800$
��Bold"�6�   $��22(�6�   $��39��6�   $	��144��6�   �� 5      -   5         ��Name$ durum��5      $��
0,68,0,275q�6+   $
��Wrap=�6?    �� TextAlignment$ Center=�6S   $
��Bold"�6j   $��17(�6|   
�������6�   5      5      5      5      