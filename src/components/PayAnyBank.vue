<template>
    <div id="any-bank">
        <div id="card">
            <div id="credit-cards">
                <i class="fa fa-cc-visa fa-fw fa-2x"></i>
                <i class="fa fa-cc-mastercard fa-fw fa-2x"></i>
                <i class="fa fa-globe fa-fw fa-2x"></i>
            </div>
            <form>
                <div class="form-group"
                     :class="{'form-group--error': $v.creditForm.cardNumber.$error }"
                     id="card-number-field">
                    <the-mask
                            mask="#### #### #### ####"
                            type="text"
                            placeholder="Номер карты"
                            class="form-control"
                            v-model.trim="$v.creditForm.cardNumber.$model"
                            id="card-number"
                    />
                    <div class="error" v-if="!$v.creditForm.cardNumber.required">Номер карты обязателен.</div>
                    <div class="error" v-if="!$v.creditForm.cardNumber.and">Номер карты состоит ровно из {{ $v.creditForm.cardNumber.$params.and.$sub[0].min }} цифр.</div>
                </div>
                <div class="flex-container">
                    <div class="form-group"
                         :class="{'form-group--error': $v.creditForm.expirationDate.$error }"
                         id="expiration-date-field">
                        <the-mask
                                mask='F#/G#' :tokens="{
                            F : {
                            pattern: /[0-1]/
                            },
                            G: {
                            pattern: /[1-3]/
                            },
                            '#': {pattern: /\d/}
                            }"
                                type="text"
                                masked
                                placeholder="MM/ГГ"
                                class="form-control"
                                v-model.trim="$v.creditForm.expirationDate.$model"
                                id="expiration-date"
                        />
                        <div class="error" v-if="!$v.creditForm.expirationDate.required">Срок действия обязателен.</div>
                        <div class="error" v-if="!$v.creditForm.expirationDate.dateValidator">Неверный формат даты.</div>
                    </div>
                    <div class="form-group"
                         :class="{'form-group--error': $v.creditForm.cvcCode.$error }"
                         id="cvc-field">
                        <the-mask
                                mask="###"
                                type="text"
                                placeholder="CVC"
                                class="form-control"
                                v-model.trim="$v.creditForm.cvcCode.$model"
                                id="cvc"
                        />
                        <div class="error" v-if="!$v.creditForm.cvcCode.required">Код обязателен.</div>
                        <div class="error" v-if="!$v.creditForm.cvcCode.and">Длина должна быть {{$v.creditForm.cvcCode.$params.and.$sub[0].max}}.</div>
                    </div>
                </div>
            </form>
        </div>
        <div id="payment">
            <form >
                <div class="form-group"
                     :class="{'form-group--error': $v.paymentForm.paymentSum.$error }">
                    <label for="paymentSum">Сумма</label>
                    <the-mask
                            id="paymentSum"
                            mask="#####"
                            type="text"
                            class="form-control"
                            v-model.trim="$v.paymentForm.paymentSum.$model"
                            placeholder="от 1 000 до 75 000 Р"
                    />
                    <div class="error" v-if="!$v.paymentForm.paymentSum.required">Сумма обязателена.</div>
                    <div class="error" v-if="!$v.paymentForm.paymentSum.between">Сумма должна быть между {{$v.paymentForm.paymentSum.$params.between.min}} и {{$v.paymentForm.paymentSum.$params.between.max}}.</div>
                </div>
                <div class="form-group"
                     :class="{'form-group--error': $v.paymentForm.paymentComment.$error }">
                    <label for="paymentComment">Комментарий</label>
                    <input
                            id="paymentComment"
                            maxlength="150"
                            type="text"
                            class="form-control"
                            placeholder="до 150 символов"
                            v-model.trim="$v.paymentForm.paymentComment.$model"
                    />
                    <div class="error" v-if="!$v.paymentForm.paymentComment.maxLength">Не больше {{$v.paymentForm.paymentComment.$params.maxLength.max}} символов.</div>
                </div>
                <div class="form-group"
                     :class="{'form-group--error': $v.paymentForm.paymentMail.$error }">
                    <label for="paymentMail">Ваша эл.почта</label>
                    <input
                            type="email"
                            id="paymentMail"
                            class="form-control"
                            placeholder="для квитанций об оплате"
                            v-model.trim="$v.paymentForm.paymentMail.$model"
                    />
                    <div class="error" v-if="!$v.paymentForm.paymentMail.required">Почта обязателена.</div>
                    <div class="error" v-if="!$v.paymentForm.paymentMail.email">Неправильный формат адреса.</div>
                </div>
                <input type="hidden" name="CSRFToken" :value="$parent.$parent.$parent.token"/>
            </form>
            <div class="submit-button">
                <button
                        type="submit"
                        class="submit-button__title"
                        :disabled="isDisabled"
                        @click.prevent="submit">Заплатить</button>
                <span class="submit-button__bg"></span>
            </div>
        </div>
    </div>
</template>

<script>
import {
  and,
  between,
  maxLength,
  minLength,
  email,
  required
} from "vuelidate/lib/validators";
import axios from "axios";

function dateValidator(value) {
  var splitted = value.split("/");
  return (
    value.length === 0 ||
    (parseInt(splitted[0]) < 13 &&
      parseInt(splitted[1]) > 18 &&
      parseInt(splitted[1]) < 36)
  );
}

export default {
  name: "PayAnyBank",
  data() {
    return {
      creditForm: {
        cardNumber: "",
        expirationDate: "",
        cvcCode: ""
      },
      paymentForm: {
        paymentSum: "",
        paymentComment: "",
        paymentMail: ""
      }
    };
  },
  methods: {
    submit() {
      console.log("submit");

      const content = {
        cardNumber: this.creditForm.cardNumber,
        expirationDate: this.creditForm.expirationDate,
        cvcCode: this.creditForm.cvcCode,
        totalCost: this.paymentForm.paymentSum,
        comment: this.paymentForm.paymentComment,
        email: this.paymentForm.paymentMail
      };

      axios("http://api.sharkbank.ru/card-payment", {
        method: "post",
        data: { body: content },
        withCredentials: true,
        headers: { "X-CSRF-Token": this.$parent.$parent.$parent.token }
      })
        .then(response => {
          if (response.status === 200) {
            this.$notify({
              group: "foo",
              type: "success",
              title: "Отправка формы",
              text: "Все прошло успешно! Спасибо."
            });
          }
        })
        .catch(e => {
          this.$notify({
            group: "foo",
            type: "error",
            title: "Отправка формы: " + e,
            text: "Что-то пошло не так!"
          });
          this.$parent.$parent.$emit("authenticated", false);
          this.$router.replace({ name: "login" });
        });

      this.creditForm.cardNumber = "";
      this.creditForm.expirationDate = "";
      this.creditForm.cvcCode = "";
      this.paymentForm.paymentSum = "";
      this.paymentForm.paymentComment = "";
      this.paymentForm.paymentMail = "";

      this.$v.$reset();
    }
  },
  computed: {
    isDisabled() {
      return this.$v.creditForm.$invalid || this.$v.paymentForm.$invalid;
    }
  },
  validations: {
    creditForm: {
      cardNumber: {
        required,
        and: and(minLength(16), maxLength(16))
      },
      expirationDate: {
        required,
        and: and(maxLength(5), minLength(5)),
        dateValidator
      },
      cvcCode: {
        required,
        and: and(maxLength(3), minLength(3))
      }
    },
    paymentForm: {
      paymentSum: {
        required,
        between: between(1000, 75000)
      },
      paymentComment: {
        maxLength: maxLength(150)
      },
      paymentMail: {
        required,
        email
      }
    }
  }
};
</script>

<style>
#any-bank {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  width: 100%;
  margin-top: 5%;
  margin-bottom: 5%;
}

#credit-cards {
  display: flex;
  justify-content: flex-end;
}

#credit-cards .fa {
  font-size: 20px;
}

#card {
  width: 350px;
  height: 200px;
  margin-top: 1%;
  padding: 20px;
  background: lightgray;
  border-radius: 10px;
  -webkit-box-shadow: 0 6px 16px 0 rgba(163, 157, 163, 1);
  -moz-box-shadow: 0 6px 16px 0 rgba(163, 157, 163, 1);
  box-shadow: 0 6px 16px 0 rgba(163, 157, 163, 1);
}

#card #card-number-field #card-number {
  width: 100%;
  margin-top: 40px;
  margin-bottom: 0;
}

#card .flex-container {
  margin-top: 5%;
  justify-content: space-between;
}

#card #cvc-field {
  width: 30%;
}

#card #cvc-field #cvc {
  width: 100%;
  margin-left: auto;
}

#card #expiration-date-field #expiration-date {
  width: 100%;
}

#card .form-group > .error {
  position: absolute;
}

#payment {
  width: 50%;
  height: 200px;
  margin: 2% 2% 2% 5%;
}

#payment form {
  display: flex;
  flex-direction: column;
}

#payment .form-group {
  position: relative;
  display: flex;
  margin-bottom: 4%;
}

#payment .form-group .form-control {
  margin-left: auto;
  display: block;
  width: 58%;
}

#payment .form-group label {
  color: dimgray;
  display: block;
  width: 40%;
  cursor: default;
}

#payment .form-group > .error {
  position: absolute;
  right: 20%;
  bottom: -13px;
  padding-top: 0;
}

.form-control {
  height: 30px;
  border: 0;
  outline: 0;
  background: transparent;
  border-bottom: 1px solid dimgray;
}

.error {
  color: red;
  font-size: 0.4em;
}

#any-bank .submit-button {
  margin-top: 1%;
}

.submit-button:after,
.submit-button:before {
  position: absolute;
  z-index: 0;
  display: block;
  left: 0;
  right: 0;
  height: 16px;
  border-left: 2px solid;
  border-right: 2px solid;
  content: "";
  border-color: #231f20;
}
.submit-button:before {
  top: 0;
  border-top: 2px solid;
}
.submit-button:after {
  bottom: 0;
  border-bottom: 2px solid;
}

.submit-button__bg {
  -webkit-transition: opacity 0.3s ease-in-out 0.2s;
  -o-transition: opacity 0.3s ease-in-out 0.2s;
  transition: opacity 0.3s ease-in-out 0.2s;
}

.submit-button__bg {
  position: absolute;
  top: 0;
  left: -10px;
  right: 0;
  bottom: -8px;
  background: url("../assets/water_button.png") left 5px no-repeat;
  background-size: 100% 100%;
  opacity: 0.9;
}
.submit-button {
  display: inline-block;
  position: relative;
  text-align: center;
}
.submit-button__title {
  display: inline-block;
  position: relative;
  font-weight: 700;
  cursor: pointer;
  padding: 10px 30px;
  text-align: center;
  white-space: nowrap;
  vertical-align: middle;
  text-decoration: none;
  color: #231f20;
  z-index: 1;
  width: 100%;
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
  background: transparent;
  border: none;
}
.submit-button__title {
  padding: 15px 35px;
}

.submit-button__title:disabled {
  cursor: default;
  opacity: 0.3;
}
.submit-button__title:disabled ~ .submit-button__bg {
  opacity: 0.3;
}

@media (max-width: 968px) {
  #any-bank {
    flex-direction: column;
  }
  #payment {
    width: 100%;
    margin-left: 0;
    margin-top: 5%;
    min-width: 400px;
  }

  #payment .form-group label {
    font-size: smaller;
  }
}
</style>
