<template>
    <div id="request-payment">
        <span class="description-span">Создайте платёжку, а Индивидуальный предприниматель Швецова Мария Влерьевна подпишет её у себя в интернет-банке</span>
        <form>
            <div class="form-group"
                 :class="{'form-group--error': $v.reqFromPayer.$error }">
                <label for="from_req">От кого</label>
                <the-mask
                        :mask="['##########','############']"
                        class="form-control"
                        placeholder="ИНН или название платильщика"
                        id="from_req"
                        type="text"
                        v-model="reqFromPayer"
                />
                <div class="error" v-if="!$v.reqFromPayer.required">ИНН обязателено.</div>
                <div class="error" v-if="!$v.reqFromPayer.or">Должно быть {{$v.reqFromPayer.$params.or.$sub[0].$sub[0].min}} или {{$v.reqFromPayer.$params.or.$sub[1].$sub[0].min}}.</div>
            </div>
            <div class="form-group"
                 :class="{'form-group--error': $v.bicForReq.$error }">
                <label for="bic_req">БИК</label>
                <the-mask
                        mask='### ### ###'
                        class="form-control"
                        placeholder="БИК"
                        id="bic_req"
                        type="text"
                        v-model="bicForReq"
                />
                <div class="error" v-if="!$v.bicForReq.required">БИК обязателено.</div>
                <div class="error" v-if="!$v.bicForReq.and">Должно быть {{$v.bicForReq.$params.and.$sub[0].min}}.</div>
            </div>

            <div class="form-group"
                 :class="{'form-group--error': $v.reqAccountNumber.$error }">
                <label for="account-number_req">Номер счета</label>
                <the-mask
                        mask="##### ##### ##### #####"
                        class="form-control"
                        placeholder="Для квитанции"
                        id="account-number_req"
                        type="text"
                        v-model="reqAccountNumber"
                />
                <div class="error" v-if="!$v.reqAccountNumber.required">Номер счета обязателено.</div>
                <div class="error" v-if="!$v.reqAccountNumber.and">Должно быть {{$v.reqAccountNumber.$params.and.$sub[0].min}}.</div>
            </div>

            <div class="form-group"
                 :class="{'form-group--error': $v.reqPaymentPurpose.$error }">
                <label for="purpose-of-payment_req">За что</label>
                <input
                        class="form-control"
                        placeholder="Назначение платежа"
                        id="purpose-of-payment_req"
                        type="text"
                        v-model="reqPaymentPurpose"
                />
                <div class="error" v-if="!$v.reqPaymentPurpose.required">Обязателено.</div>
                <div class="error" v-if="!$v.reqPaymentPurpose.contains">Должно содержать инфо о НДС.</div>

                <div id="vat_labels_req">
                    <span @click="vat18">НДС 18%</span>
                    <span @click="vat10">НДС 10%</span>
                    <span @click="novat">без НДС</span>
                </div>
            </div>
            <div class="form-group"
                 :class="{'form-group--error': $v.reqTotalCost.$error }">
                <label for="total-cost_req">Сколько</label>
                <the-mask
                        mask="#####"
                        class="form-control"
                        placeholder="от 1000 до 75000"
                        id="total-cost_req"
                        type="text"
                        v-model="reqTotalCost"
                />
                <div class="error" v-if="!$v.reqTotalCost.required">Обязателено.</div>
                <div class="error" v-if="!$v.reqTotalCost.between">Между {{$v.reqTotalCost.$params.between.min}} и {{$v.reqTotalCost.$params.between.max}}</div>
            </div>

            <div class="form-group"
                 :class="{'form-group--error': $v.reqPhoneNumber.$error }">
                <label for="phone-number_req">Номер телефона</label>
                <the-mask
                        mask="+7(###)###-##-##"
                        class="form-control"
                        placeholder="+7"
                        id="phone-number_req"
                        type="text"
                        v-model="reqPhoneNumber"
                />
                <div id="data-processing-consent"> Оставляя номер телефоны, вы соглашаетесь на обработку персональных данных</div>
            </div>
            <div class="form-group"
                 :class="{'form-group--error': $v.reqEmail.$error }">
                <label for="email_req">Эл.почта</label>
                <input
                        class="form-control"
                        placeholder="Для уведомлений об оплате"
                        id="email_req"
                        type="email"
                        v-model="reqEmail"
                />
                <div class="error" v-if="!$v.reqEmail.email">Проверьте правильность адреса</div>
            </div>
            <input type="hidden" name="CSRFToken" :value="this.$parent.$parent.token"/>
        </form>
        <div class="submit-button">
            <input
                    type="submit"
                    class="submit-button__title"
                    value="Создать платёж"
                    @click.prevent="submit"
                    :disabled="$v.$invalid"
            />
            <span class="submit-button__bg"></span>
        </div>
        <div id="clean_form" >
            <span @click="cleanForm">Очистить форму</span>
        </div>
    </div>
</template>

<script>
import {
  email,
  and,
  between,
  maxLength,
  minLength,
  or,
  required
} from "vuelidate/lib/validators";
import axios from "axios";

const contains = value =>
  value.length === 0 ||
  value.indexOf("НДС 18%") >= 0 ||
  value.indexOf("НДС 10%") >= 0 ||
  value.indexOf("Без НДС") >= 0;

export default {
  name: "RequestPayment",
  data() {
    return {
      reqFromPayer: "",
      bicForReq: "",
      reqAccountNumber: "",
      reqPaymentPurpose: "",
      reqTotalCost: "",
      reqPhoneNumber: "",
      reqEmail: ""
    };
  },
  methods: {
    submit() {
      console.log("submit");

      const content = {
        reqFromPayer: this.reqFromPayer,
        bicForReq: this.bicForReq,
        reqAccountNumber: this.reqAccountNumber,
        reqPaymentPurpose: this.reqPaymentPurpose,
        reqTotalCost: this.reqTotalCost,
        reqPhoneNumber: this.reqPhoneNumber,
        reqEmail: this.reqEmail
      };

      axios("http://api.sharkbank.ru/request-payment", {
        method: "post",
        data: { body: content },
        withCredentials: true,
        headers: { "X-CSRF-Token": this.$parent.$parent.token }
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
          this.$parent.$emit("authenticated", false);
          this.$router.replace({ name: "login" });
        });

      this.cleanForm();
    },
    vat18() {
      this.reqPaymentPurpose = "НДС 18%";
    },
    vat10() {
      this.reqPaymentPurpose = "НДС 10%";
    },
    novat() {
      this.reqPaymentPurpose = "Без НДС";
    },
    cleanForm() {
      this.$v.$reset();
      this.reqFromPayer = "";
      this.bicForReq = "";
      this.reqAccountNumber = "";
      this.reqPaymentPurpose = "";
      this.reqTotalCost = "";
      this.reqPhoneNumber = "";
      this.reqEmail = "";
    }
  },
  validations: {
    reqFromPayer: {
      required,
      or: or(
        and(minLength(10), maxLength(10)),
        and(minLength(12), maxLength(12))
      )
    },
    bicForReq: {
      required,
      and: and(minLength(9), maxLength(9))
    },
    reqAccountNumber: {
      required,
      and: and(minLength(20), maxLength(20))
    },
    reqPaymentPurpose: {
      required,
      contains
    },
    reqTotalCost: {
      required,
      between: between(1000, 75000)
    },
    reqPhoneNumber: {},

    reqEmail: {
      email
    }
  }
};
</script>

<style scoped>
#request-payment {
  width: 80%;
  margin-left: 5%;
}

#request-payment .description-span {
  display: block;
  margin-top: 5%;
  margin-bottom: 5%;
}

#request-payment form {
  display: flex;
  flex-direction: column;
}

#request-payment .form-group {
  position: relative;
  display: flex;
  margin-bottom: 4%;
}

#request-payment .form-group label {
  display: block;
  width: 28%;
  color: dimgray;
  cursor: default;
}

#request-payment .form-group .form-control {
  display: block;
  margin-left: auto;
  width: 70%;
}

#request-payment .form-group > .error {
  position: absolute;
  left: 60%;
  bottom: -13px;
  padding-top: 0;
}

#request-payment #vat_labels_req {
  position: absolute;
  display: flex;
  bottom: -20px;
  left: 30%;
  width: 28%;
  justify-content: space-between;
  font-size: xx-small;
  color: cadetblue;
}

#request-payment #vat_labels_req span {
  cursor: pointer;
}

#request-payment #data-processing-consent {
  position: absolute;
  bottom: -13px;
  left: 30%;
  font-size: 0.45em;
  color: cadetblue;
}

.submit-button__title:disabled {
  cursor: default;
  opacity: 0.3;
}
.submit-button__title:disabled ~ .submit-button__bg {
  opacity: 0.3;
}

#request-payment #clean_form {
  font-size: smaller;
  margin-top: 2%;
  text-decoration: underline;
  color: cadetblue;
  cursor: pointer;
}
@media (max-width: 940px) {
  #request-payment {
    width: 90%;
  }
  #request-payment .form-group {
    flex-direction: column;
    min-width: 400px;
  }

  #request-payment .form-group label {
    width: auto;
    font-size: smaller;
  }

  #request-payment .form-group .form-control {
    margin-left: 0;
    width: 100%;
  }

  #request-payment .form-group > .error {
    left: 45%;
  }

  #request-payment .form-group > #vat_labels_req {
    font-size: 0.45em;
    bottom: -13px;
    left: 0;
    width: 43%;
  }

  #request-payment .form-group > #data-processing-consent {
    left: 0;
  }
}
@media (max-width: 538px) {
  #request-payment .form-group > #data-processing-consent {
    font-size: 0.38em;
    bottom: -10px;
  }
}
</style>
