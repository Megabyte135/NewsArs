const arrow = document.querySelector('.arrowUP-none'),
      arrowUP = document.querySelector('.arrowUP')
  






// ==================================================================

function scroll(){
   
    window.onscroll = function showHeader(){
         if(window.pageYOffset > 50){
            arrow.classList.add('arrowUP');
         }else{
             arrow.classList.remove('arrowUP');
        }
    };
}

scroll();

// ==============================вкщз===========================================



// =================================== Редактировать ===============================================


admin.addEventListener('click', () =>{
    edit.classList.toggle('edit')
})



// =========================================================
function smooth(){
    const anchors = document.querySelectorAll('a[href*=#]');

for(let anchor of anchors){
    anchor.addEventListener("click", function(event){
        event.preventDefault();
        const blockID = anchor.getAttribute('href');
        document.querySelector('' + blockID).scrollIntoView({
            behavior: "smooth",
            block: "start"
         });
        });
       }
     }

smooth()


