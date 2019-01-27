<template>
    <div class="app">
        <notifications position="top center" group="foo"/>
        <div class="header">
            <div id="info">
                <h3>{{ companyName }}</h3>
                <div id="contacts">
                    <span id="phone_number">+78005553535</span>
                    <a href="www.mary.com">www.mary.com</a>
                    <a href="mailto:mary@shark.com"> mary@shark.com</a>
                </div>
                <div id="hidden_info">
                    <div id="about">
                        <details>
                            <summary>Информация о компании</summary>
                            <p>ИП Швецовой: супер клёвое ИП для супер клёвых клиентов</p>
                        </details>
                    </div>
                    <div id="requisites">
                        <details>
                            <summary>Показать риквизиты</summary>
                            <p>тут типа реквизиты</p>
                        </details>
                    </div>
                </div>
            </div>
            <div id="user_img"><img alt="Your photo" src="../assets/userpic.png"/></div>
        </div>
        <div class="payments-nav">
            <div class="flex-container">
                <router-link to="/pay">
                    Заплатить</router-link>
                <router-link to="/request">
                    Запросить платёж</router-link>
            </div>
        </div>
        <router-view></router-view>
        <div class="about_company">
            <h3>
                О компании {{ companyName }}
            </h3>
            <div id="company_services">
                <div id="strawberry">
                    <img
                            src="https://life-globe.com/image/cache/catalog/spain/barcelona/mercat-boqueria/mercat-boqueria-2/mercat-de-la-boqueria-barcelona-frukti-915x610.jpg"
                    />
                    <h5>Клубника на улице Рамбла</h5>
                    <div class="price">139,99 руб.</div>
                </div>
                <div id="beach">
                    <img src="https://ufa.vipgeo.ru/uploads/redactor/large/207213.jpg" />
                    <h5>Золотой пляж</h5>
                    <div class="price">35,89 руб.</div>
                </div>
                <div id="guides">
                    <img
                            src="https://espanarusa.com/files/autoupload/84/1/83/fd3qmskp394110.jpg"
                    />
                    <h5>Экскурсии</h5>
                    <div class="price">1000р/час</div>
                </div>
                <div id="boat">
                    <img
                            src="http://alandalus.ru/assets/images/Yachts/rodman-41-fly_720.jpg"
                    />
                    <h5>Яхта</h5>
                    <div class="price">2000р/час</div>
                </div>
            </div>
            <input id="trigger" type="checkbox" />
            <div class="about_text">
                Испа́ния (исп. и галис. España), официально Короле́вство Испа́ния (исп. и
                галис. Reino de España МФА [ˈreino ðe esˈpaɲa]) — суверенное государство
                на юго-западе Европы и частично в Африке, член Европейского союза и
                НАТО. Испания занимает бо́льшую часть (80 %) Пиренейского полуострова, а
                также Канарские и Балеарские острова, имеет общую площадь 504 782 км²
                (вместе с небольшими суверенными территориями на африканском побережье,
                городами Сеута и Мелилья), являясь четвёртой по величине страной в
                Европе (после России, Украины и Франции).
            </div>
            <div class="clear"></div>
            <label for="trigger" class="open_list">
                <span class="show--list">Показать все</span>
                <span class="hide--list">Скрыть</span>
            </label>
        </div>
        <div class="footer">
            <div class="flex-container">
                <a href="#" class="fa fa-facebook"></a>
                <a href="#" class="fa fa-twitter"></a>
                <a href="#" class="fa fa-vk"></a> <a href="#" class="fa fa-google"></a>
                <a href="#" class="fa fa-telegram"></a>
            </div>
        </div>
    </div>
</template>

<script>
export default {
  name: "MainPage",
  data() {
    return {
      companyName: "Индивидуальный предприниматель Швецова Мария Валерьевна"
    };
  }
};
</script>

<style>
@import url("https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css");

* {
  margin: 0;
  padding: 0;
  outline: none;
}

body,
html {
  width: 100%;
  height: 100%;
}

body {
  font-family: Verdana, Geneva, Tahoma, sans-serif;
}

h1,
h2,
h3,
h4,
h5 {
  font-family: "Franklin Gothic Medium", "Arial Narrow", Arial, sans-serif;
  font-weight: lighter;
}

h3 {
  min-width: 300px;
}

.header {
  margin: 10px 5% 2%;
  width: 90%;
  /* float: left; */
  -webkit-box-shadow: 0 8px 6px -4px gray;
  -moz-box-shadow: 0 8px 6px -4px gray;
  -ms-box-shadow: 0 8px 6px -4px gray;
  -o-box-shadow: 0 8px 6px -4px gray;
  box-shadow: 0 8px 6px -4px gray;
  border-radius: 10px 30px;
}

.header #info {
  /* float: left; */
  display: inline-block;
  width: 70%;
  margin: 2%;
}

.header #info #contacts {
  margin: 2% 0 2% 0;
}

.header #info a {
  text-decoration: none;
  color: cadetblue;
  margin-right: 5%;
}

.header #info #contacts #phone_number {
  color: gray;
  margin-right: 5%;
}

.header #hidden_info > div {
  margin-bottom: 5%;
}

.header #hidden_info summary {
  color: cadetblue;
  cursor: pointer;
}

.header #hidden_info p {
  margin: 2% 0 0 2%;
}

.header details summary::-webkit-details-marker {
  display: none;
}

.header #user_img {
  float: right;
  display: inline-block;
  width: 20%;
  margin: 2%;
}

.header #user_img img {
  float: left;
  width: 100%;
  max-width: 200px;
  border-radius: 10px;
}

@media (max-width: 768px) {
  .header {
    min-width: 300px;
  }
  .header #info {
    width: 100%;
  }

  .header #user_img {
    float: none;
    width: 100%;
  }

  .header #user_img img {
    float: none;
    border-radius: 30px;
  }
}

.flex-container {
  display: flex;
  justify-content: flex-start;
}

.payments-nav {
  width: 90%;
  margin: 5% 5% 2% 7%;
}

.payments-nav a {
  margin-right: 5%;
  color: cadetblue;
  text-decoration: none;
  font-family: "Franklin Gothic Medium", "Arial Narrow", Arial, sans-serif;
  font-weight: bolder;
}

.payments-nav .router-link-active {
  color: black;
}

@media (max-width: 600px) {
  .payments-nav .flex-container {
    flex-direction: column;
  }
}

.about_company {
  width: 90%;
  margin: 10px 5% 2%;
}

.about_company h3 {
  margin: 2%;
  margin-top: 5%;
}

.about_company #company_services {
  min-width: 300px;
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
}

.about_company #company_services > * {
  width: 23%;
  margin: 1%;
  -webkit-box-shadow: 2px 4px 4px 2px gray;
  -moz-box-shadow: 2px 4px 4px 2px gray;
  -ms-box-shadow: 2px 4px 4px 2px gray;
  -o-box-shadow: 2px 4px 4px 2px gray;
  box-shadow: 2px 4px 4px 2px gray;
  border-radius: 10px;
}

.about_company #company_services > *:hover {
  width: 25%;
  margin: 0;
  flex-wrap: nowrap;
}

.about_company #company_services img {
  width: 90%;
  height: 55%;
  min-width: 140px;
  min-height: 95px;
  margin: 5%;
  margin-bottom: 2%;
  border: 2px solid cadetblue;
}

.about_company h5,
.price {
  height: 20%;
  width: 90%;
  margin: 0 5% 0 5%;
}

.about_company .price {
  margin: 5%;
  font-size: 70%;
  color: gray;
}

@media (max-width: 768px) {
  .about_company #company_services > * {
    width: 48%;
  }
  .about_company #company_services > *:hover {
    width: 50%;
  }
}

@media (max-width: 600px) {
  .about_company #company_services > * {
    width: 98%;
  }
  .about_company #company_services > *:hover {
    width: 100%;
  }
}

.footer {
  min-width: 400px;
  margin: 5%;
}

.footer .fa {
  margin: 0 2% 0 2%;
  padding: 20px;
  font-size: 30px;
  width: 30px;
  text-align: center;
  text-decoration: none;
  border-radius: 50%;
}

.footer .fa:hover {
  opacity: 0.7;
}

.footer .fa-facebook {
  background: #3b5998;
  color: white;
}

.footer .fa-twitter {
  background: #55acee;
  color: white;
}

.footer .fa-vk {
  background: #4c75a3;
  color: white;
}

.footer .fa-google {
  background: #dd4b39;
  color: white;
}

.footer .fa-telegram {
  background: #0084ff;
  color: white;
}

@media (max-width: 600px) {
  .footer .fa {
    padding: 10px;
    font-size: 25px;
    width: 25px;
  }
}

.about_text {
  margin: 2%;
  width: 90%;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}

.open_list > span {
  margin: 2%;
  width: 90%;
  text-decoration: underline;
  color: #6d87b4;
}

#trigger {
  display: none;
}

label {
  cursor: pointer;
}

.hide--list {
  display: none;
}

#trigger:checked + .about_text {
  overflow: visible;
  white-space: normal;
  text-overflow: ellipsis;
}

#trigger:checked ~ .open_list .hide--list {
  display: block;
}

#trigger:checked ~ .open_list .show--list {
  display: none;
}
</style>
