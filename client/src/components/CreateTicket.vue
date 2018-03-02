<template>
  <div class="top row justify-center"> 
    <h1>Create Ticket</h1>
    <q-stepper :vertical="vertical" class="stepper" color="secondary" ref="stepper" alternative-labels>
      <q-step default name="first" title="Choose a Technician">
        <q-select
        v-model="tech"
        float-label="Choose a Technician"
        radio
        :options="techOptions"/>
        <!-- Navigation for this step at the end of QStep-->
        <q-stepper-navigation>
          <q-btn color="secondary" @click="$refs.stepper.next()">Continue</q-btn>
        </q-stepper-navigation>
      </q-step>
      
      <q-step name="second" title="Choose a Room">
        <q-select
          v-model="room"
          float-label="Choose a Room"
          radio
        :options="roomOptions"/>
        <q-stepper-navigation>
          <q-btn color="secondary" @click="$refs.stepper.next()">Next</q-btn>
          <q-btn color="secondary" flat @click="$refs.stepper.previous()">Back</q-btn>
        </q-stepper-navigation>
      </q-step>

      <q-step name="third" title="Choose Malfunctioning Machine">
        <q-select
          v-model="machine"
          float-label="Choose a Machine"
          radio
        :options="machineOptions"/>
        <q-stepper-navigation>
          <q-btn color="secondary" @click="$refs.stepper.next()">Next</q-btn>
          <q-btn color="secondary" flat @click="$refs.stepper.previous()">Back</q-btn>
        </q-stepper-navigation>
      </q-step>

      <q-step name="fifth" title="Explain Issue">
        <q-input
          v-model="issue"
          type="textarea"
          float-label="Textarea"
          :max-height="100"
        />
        <q-stepper-navigation>
          <q-btn color="secondary" @click="$refs.stepper.next()">Next</q-btn>
          <q-btn color="secondary" flat @click="$refs.stepper.previous()">Back</q-btn>
        </q-stepper-navigation>
      </q-step>
  
      <q-step name="fourth" title="Review and Finalize">
        <div>Tech: {{tech}}</div>
        <div>Room: {{room}}</div>
        <div>Machine: {{machine}}</div>
        <div>Issue: {{issue}}</div>
        <q-stepper-navigation>
          <q-btn color="secondary" @click="$refs.stepper.goToStep('first')">Restart</q-btn>
          <q-btn color="secondary" flat @click="$refs.stepper.previous()">Back</q-btn>
        </q-stepper-navigation>
      </q-step>
    </q-stepper>
  </div>
</template>

<script>
import * as Firebase from 'firebase'
import {
  Alert,
  QField,
  QLayout,
  QToolbar,
  QToolbarTitle,
  QBtn,
  QIcon,
  QList,
  QListHeader,
  QItem,
  QItemSide,
  QItemMain,
  QPopover,
  QInput,
  QCard,
  QCardTitle,
  QCardActions,
  QCardMedia,
  QRating,
  QCardMain,
  QCardSeparator,
  QModal,
  QStep,
  QStepper,
  QStepperNavigation,
  QSelect
} from 'quasar'

export default {
  name: 'createticket',
  components: {
    Alert,
    QField,
    QLayout,
    QToolbar,
    QToolbarTitle,
    QBtn,
    QIcon,
    QList,
    QListHeader,
    QItem,
    QItemSide,
    QItemMain,
    QPopover,
    QInput,
    QCard,
    QCardTitle,
    QCardActions,
    QCardMedia,
    QRating,
    QCardMain,
    QCardSeparator,
    QModal,
    QStep,
    QStepper,
    QStepperNavigation,
    QSelect
  },
  data () {
    return {
      tech: '',
      room: '',
      machine: '',
      issue: '',
      techs: [],
      sites: [],
      machines: [],
      rooms: [],
      vertical: false
    }
  },
  methods: {
  },
  computed: {
    techOptions () {
      var techs = []
      this.techs.forEach(element => {
        techs.push({label: element.firstname + ' ' + element.lastname, value: element.firstname + ' ' + element.lastname})
      })
      return techs
    }
  },
  mounted () {
    var techRef = Firebase.database().ref('technicians')
    this.techs = []
    techRef.on('value', snapshot => {
      snapshot.forEach(userSnapshot => {
        this.techs.push(userSnapshot.val())
      })
    })
    var isMobile = {
      Android: function () {
        return navigator.userAgent.match(/Android/i)
      },
      BlackBerry: function () {
        return navigator.userAgent.match(/BlackBerry/i)
      },
      iOS: function () {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i)
      },
      Opera: function () {
        return navigator.userAgent.match(/Opera Mini/i)
      },
      Windows: function () {
        return navigator.userAgent.match(/IEMobile/i)
      },
      any: function () {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows())
      }
    }
    if (isMobile.iOS()) {
      this.vertical = true
    }
  }
}
</script>

<style scoped>
.top {
  padding-top: 1%
}
.stepper {
  width: 90%
}
</style>

