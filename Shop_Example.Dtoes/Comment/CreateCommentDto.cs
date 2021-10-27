using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Dtoes.Comment
{
    public class CreateCommentDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required(ErrorMessage ="لطفا عنوان نظر خود را وارد کنید")]
        [MaxLength(70,ErrorMessage ="عنوان نظر شما حداکثر میتواند 70 کاراکتر باشد")]
        public string Title { get; set; }
        [Required(ErrorMessage ="لطفا متن نظر خود را وارد کنید")]
        [MaxLength(400,ErrorMessage ="متن نظر شما حداکثر میتواند 400 کاراکتر باشد")]
        public string Text { get; set; }

        [Range(1,5)]
        public byte EasyUse { get; set; }//راحتی در استفاده
        [Range(1,5)]
        public byte Beauty { get; set; }//زیبایی
        [Range(1,5)]
        public byte QualityBuild { get; set; }//کیفیت ساخت
        [Range(1,5)]
        public byte Affordable { get; set; }//قیمت نسبت به کیفیت
        [Range(1,5)]
        public byte Innovation { get; set; }//نوآوری
        [Range(1,5)]
        public byte Ability { get; set; }//قابلیت ها


        public bool Suggestion { get; set; } = true;

        public string[] goods { get; set; }
        public string[] bads { get; set; }
    }
}

//< div class= "comments-add" >
 
//                                 < div class= "comments-add-row" >
  
//                                      < div class= "col-lg-6 col-md-6 col-xs-12 pull-right" >
   
//                                           < div class= "comments-add-col-form" >
    
//                                                < div class= "form-comment" >
     
//                                                     < div class= "col-md-12 col-sm-12" >
      
//                                                          < div class= "form-ui" >
       

//                                                                   < div class= "row" >
        
//                                                                        < div class= "col-12" >
         
//                                                                             < div class= "form-row-title mb-2" > عنوان نظر شما(اجباری)</ div >
            
//                                                                                < div class= "form-row" >
             
//                                                                                     < input class= "input-ui pr-2 input-validation-error" placeholder = "عنوان نظر خود را بنویسید" type = "text" data - val = "true" data - val - maxlength = "عنوان نظر شما حداکثر میتواند 70 کاراکتر باشد" data - val - maxlength - max = "70" data - val - required = "لطفا عنوان نظر خود را وارد کنید" id = "Title" maxlength = "70" name = "Title" value = "" aria - describedby = "Title-error" aria - invalid = "true" >
                                                       
//                                                                                                                               < span class= "text-danger field-validation-error" data - valmsg -for= "Title" data - valmsg - replace = "true" >< span id = "Title-error" class= "" > لطفا عنوان نظر خود را وارد کنید</span></span>
//                                                                    </div>
//                                                                </div>
//                                                                <div class= "col-12 form-comment-title--positive mt-4" >
//                                                                    < div class= "form-row-title mb-2 pr-3" >
//                                                                         نقاط قوت
//                                                                     </ div >
 
//                                                                     < div id = "advantages" class= "form-row" >
    
//                                                                            < div class= "ui-input--add-point" >
     
//                                                                                 < input name = "goods" class= "input-ui pr-2 ui-input-field valid" type = "text" id = "advantage-input" autocomplete = "off" value = "" aria - invalid = "false" >
                 
//                                                                                             < button class= "ui-input-point js-icon-form-add" type = "button" style = "display: none;" ></ button >
                     
//                                                                                             </ div >
                     
//                                                                                             < div class= "form-comment-dynamic-labels js-advantages-list" >< div class= "ui-dynamic-label ui-dynamic-label--positive js-advantage-item" >
//                         sdfsdfsdfsdfsdf < button type = "button" class= "ui-dynamic-label-remove js-icon-form-remove" ></ button >
//                             < input type = "hidden" name = "comment[advantages][]" value = "sdfsdfsdfsdfsdf" >
//                                  </ div ></ div >
                                  
//                                                                                                      </ div >
                                  
//                                                                                                  </ div >
                                  
//                                                                                                  < div class= "col-12 form-comment-title--negative mt-2" >
                                   
//                                                                                                       < div class= "form-row-title mb-2 pr-3" >
//                                                                                                            نقاط ضعف
//                                                                                                        </ div >
                                    
//                                                                                                        < div id = "disadvantages" class= "form-row" >
                                       
//                                                                                                               < div class= "ui-input--add-point" >
                                        
//                                                                                                                    < input name = "bads" class= "input-ui pr-2 ui-input-field valid" type = "text" id = "disadvantage-input" autocomplete = "off" value = "" aria - invalid = "false" >
                                                    
//                                                                                                                                < button class= "ui-input-point js-icon-form-add" type = "button" style = "display: none;" ></ button >
                                                        
//                                                                                                                                </ div >
                                                        
//                                                                                                                                < div class= "form-comment-dynamic-labels js-disadvantages-list" >< div class= "ui-dynamic-label ui-dynamic-label--negative js-disadvantage-item" >
//                                                            rereer < button type = "button" class= "ui-dynamic-label-remove js-icon-form-remove" ></ button >
//                                                                < input type = "hidden" name = "comment[disadvantages][]" value = "rereer" >
//                                                                     </ div ></ div >
                                                                     
//                                                                                                                                         </ div >
                                                                     
//                                                                                                                                     </ div >
                                                                     
//                                                                                                                                     < div class= "col-12 mt-3" >
                                                                      
//                                                                                                                                          < div class= "form-row-title mb-2" > متن نظر شما(اجباری)</ div >
                                                                         
//                                                                                                                                             < div class= "form-row" >
                                                                          
//                                                                                                                                                  < textarea class= "input-ui pr-2 pt-2 input-validation-error" rows = "5" placeholder = "متن خود را بنویسید" style = "height:120px;" data - val = "true" data - val - maxlength = "متن نظر شما حداکثر میتواند 400 کاراکتر باشد" data - val - maxlength - max = "400" data - val - required = "لطفا متن نظر خود را وارد کنید" id = "Text" maxlength = "400" name = "Text" aria - describedby = "Text-error" ></ textarea >
                                                                                                                
//                                                                                                                                                                                        < span class= "text-danger field-validation-error" data - valmsg -for= "Text" data - valmsg - replace = "true" >< span id = "Text-error" class= "" > لطفا متن نظر خود را وارد کنید</span></span>

//                                                                    </div>
//                                                                </div>
//                                                                <div class= "col-12" >
//                                                                    < div class= "form-row-title mb-2" > پیشنهاد میشود </ div >
    
//                                                                        < div class= "form-row" >
     
//                                                                             < input value = "true" class= "input-ui pr-2 valid" type = "checkbox" data - val = "true" data - val - required = "The Suggestion field is required." id = "Suggestion" name = "Suggestion" aria - describedby = "Suggestion-error" >
                         
//                                                                                             </ div >
                         
//                                                                                         </ div >
                         
//                                                                                         < br >
                         
//                                                                                         < br >
                         
//                                                                                         < br >
                         
//                                                                                         < div class= "col-12 mt-5 px-0" >
                          
//                                                                                              < button class= "btn comment-submit-button" >
//                                                                                                   ثبت نظر
//                                                                                               </ button >
                           
//                                                                                           </ div >
                           
//                                                                                       </ div >
                           
//                                                                                   < input name = "Suggestion" type = "hidden" value = "false" >
                                
//                                                                                    </ div >
                                
//                                                                                </ div >
                                
//                                                                            </ div >
                                
//                                                                        </ div >
                                
//                                                                    </ div >
                                
//                                                                    < div class= "col-lg-6 col-md-6 col-xs-12 pull-left" >
                                 
//                                                                         < div class= "comments-add-col-content" >
                                  
//                                                                              < h3 > دیگران را با نوشتن نظرات خود، برای انتخاب این محصول راهنمایی کنید.</h3>
//                                            <div>
//                                                <p>لطفا پیش از ارسال نظر، خلاصه قوانین زیر را مطالعه کنید:</ p >< p >
//                                                    فارسی بنویسید و از کیبورد فارسی استفاده کنید. بهتر است از فضای خالی (Space)
//                                                    بیش‌از‌حدِ معمول، شکلک یا ایموجی استفاده نکنید و از کشیدن حروف یا کلمات با
//                                                    صفحه‌کلید بپرهیزید.
//                                                </p><p>
//                                                    به کاربران و سایر اشخاص احترام بگذارید. پیام‌هایی که شامل محتوای توهین‌آمیز و
//                                                    کلمات نامناسب باشند، حذف می‌شوند.
//                                                </p><p>
//                                                    هرگونه نقد و نظر در خصوص سایت دیجی‌کالا، خدمات و درخواست کالا را با ایمیل
//                                                    <a href="mailto:info@kalamarket.com">
//                                                        info@kalamarket.com
//                                                    </a>
//                                                    یا با شماره‌ی

//                                                    <a href="tel: +982161930000">
//                                                        ۶۱۹۳۰۰۰۰ - ۰۲۱
//                                                    </a>
//                                                    در میان بگذارید و از نوشتن آن‌ها در بخش نظرات خودداری کنید.
//                                                </p>
//                                            </div>
//                                        </div>
//                                    </div>
//                                </div>
//                            </div>