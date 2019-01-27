<template>
    <div id="your-bank">
        <span class="description-span">Сформируйте платёжку и загрузите ее в свой банк для подписи</span>
        <form>

            <div class="form-group"
                 :class="{'form-group--error': $v.fromPayer.$error }">
                <label for="from">От кого</label>
                <the-mask
                    :mask="['##########','############']"
                    class="form-control"
                    placeholder="ИНН или название платильщика"
                    id="from"
                    type="text"
                    v-model="$v.fromPayer.$model"
                />
                <div class="error" v-if="!$v.fromPayer.required">ИНН обязателено.</div>
                <div class="error" v-if="!$v.fromPayer.or">Должно быть {{$v.fromPayer.$params.or.$sub[0].$sub[0].min}} или {{$v.fromPayer.$params.or.$sub[1].$sub[0].min}}.</div>
            </div>
            <div class="form-group"
                 :class="{'form-group--error': $v.bicNumber.$error }">
                <label for="bic">БИК</label>
                <the-mask
                        mask='### ### ###'
                        class="form-control"
                        placeholder="БИК"
                        id="bic"
                        type="text"
                        v-model="bicNumber"
                />
                <div class="error" v-if="!$v.bicNumber.required">БИК обязателено.</div>
                <div class="error" v-if="!$v.bicNumber.and">Должно быть {{$v.bicNumber.$params.and.$sub[0].min}}.</div>
            </div>
            <div class="form-group"
                 :class="{'form-group--error': $v.accountNumber.$error}">
                <label for="account-number">Номер счета</label>
                <the-mask
                        mask="##### ##### ##### #####"
                        class="form-control"
                        placeholder="Для квитанции"
                        id="account-number"
                        type="text"
                        v-model="accountNumber"
                />
                <div class="error" v-if="!$v.accountNumber.required">Номер счета обязателено.</div>
                <div class="error" v-if="!$v.accountNumber.and">Должно быть {{$v.accountNumber.$params.and.$sub[0].min}}.</div>
            </div>
            <div class="form-group"
                 :class="{'form-group--error': $v.paymentPurpose.$error}">
                <label for="purpose-of-payment">За что</label>
                <input
                        class="form-control"
                        placeholder="Назначение платежа"
                        id="purpose-of-payment"
                        type="text"
                        v-model="paymentPurpose"
                />
                <div class="error" v-if="!$v.paymentPurpose.required">Обязателено.</div>
                <div class="error" v-if="!$v.paymentPurpose.contains">Должно содержать инфо о НДС.</div>
                <div id="vat_labels">
                    <span @click="vat18">НДС 18%</span>
                    <span @click="vat10">НДС 10%</span>
                    <span @click="novat">без НДС</span>
                </div>
            </div>
            <div class="form-group"
                 :class="{'form-group--error': $v.totalCost.$error}">
                <label for="total-cost">Сколько</label>
                <the-mask
                        mask="#####"
                        class="form-control"
                        placeholder="от 1000 до 75000"
                        id="total-cost"
                        type="text"
                        v-model="totalCost"
                />
                <div class="error" v-if="!$v.totalCost.required">Обязателено.</div>
                <div class="error" v-if="!$v.totalCost.between">Между {{$v.totalCost.$params.between.min}} и {{$v.totalCost.$params.between.max}}</div>
            </div>
            <input type="hidden" name="CSRFToken" :value="$parent.$parent.$parent.token"/>
        </form>
        <div class="submit-button">
            <input
                    type="submit"
                    class="submit-button__title"
                    value="Получить файл для интернет-банка"
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
/* eslint-disable no-unused-vars */

import {
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
  name: "PayYourBank",
  data() {
    return {
      fromPayer: "",
      bicNumber: "",
      accountNumber: "",
      paymentPurpose: "",
      totalCost: ""
    };
  },
  validations: {
    fromPayer: {
      required,
      or: or(
        and(minLength(10), maxLength(10)),
        and(minLength(12), maxLength(12))
      )
    },
    bicNumber: {
      required,
      and: and(minLength(9), maxLength(9))
    },
    accountNumber: {
      required,
      and: and(minLength(20), maxLength(20))
    },
    paymentPurpose: {
      required,
      contains
    },
    totalCost: {
      required,
      between: between(1000, 75000)
    }
  },
  methods: {
    vat18() {
      this.paymentPurpose = "НДС 18%";
    },
    vat10() {
      this.paymentPurpose = "НДС 10%";
    },
    novat() {
      this.paymentPurpose = "Без НДС";
    },
    cleanForm() {
      this.$v.$reset();
      this.fromPayer = "";
      this.bicNumber = "";
      this.accountNumber = "";
      this.paymentPurpose = "";
      this.totalCost = "";
    }

  }
};
</script>

<style scoped>
#your-bank {
  width: 90%;
}
#your-bank .description-span {
  display: block;
  margin-top: 5%;
  margin-bottom: 5%;
}

#your-bank form {
  display: flex;
  flex-direction: column;
}

#your-bank .form-group {
  position: relative;
  display: flex;
  margin-bottom: 4%;
}

#your-bank .form-group label {
  display: block;
  width: 20%;
  color: dimgray;
  cursor: default;
}

#your-bank .form-group .form-control {
  display: block;
  margin-left: auto;
  width: 70%;
}

#your-bank .form-group > .error {
  position: absolute;
  left: 60%;
  bottom: -13px;
  padding-top: 0;
}

#your-bank #vat_labels {
  position: absolute;
  bottom: -20px;
  left: 30%;
  display: flex;
  width: 28%;
  justify-content: space-between;
  font-size: xx-small;
  color: cadetblue;
}

#your-bank #vat_labels span {
  cursor: pointer;
}

#your-bank #clean_form {
  font-size: smaller;
  margin-top: 2%;
  text-decoration: underline;
  color: cadetblue;
  cursor: pointer;
}

@media (max-width: 878px) {
  #your-bank .form-group {
    flex-direction: column;
    min-width: 400px;
  }
  #your-bank .form-group label {
    font-size: smaller;
  }
  #your-bank .form-group .form-control {
    margin-left: 0;
    width: 100%;
  }
  #your-bank .form-group > .error {
    left: 45%;
  }
  #your-bank .form-group #vat_labels {
    font-size: 0.45em;
    bottom: -13px;
    left: 0;
    width: 43%;
  }
}
</style>
