<template>
  <div>
    <q-pull-to-refresh :handler="refresher">
      <div class="row justify-center"> 
        <q-card style="width: 500px">
          <q-card-media>
            <img :src="photoUrl">
            <q-progress :percentage="progressUpload" stripe animate style="height: 20px" />
            <div class="progress-bar" :style="{ width: progressUpload + '%'}">{{ progressUpload }}%</div>
            <label class="custom-file-upload">
              <input type="file" accept="image/*" @change="uploadPhoto($event.target.files)"/>Upload Profile Picture
            </label>
          </q-card-media>
          <q-card-title>
            {{user.displayName}}
            <q-rating slot="subtitle" v-model="stars" :max="5" />
            <div slot="right" class="row items-center">
              <q-icon name="place" /> 250 ft
            </div>
          </q-card-title>
          <q-card-main>
            <p>User</p>
            <p class="text-faded">Small plates, salads & sandwiches in an intimate setting.</p>
          </q-card-main>
          <q-card-separator />
          <q-card-actions>
            <q-btn flat round small><q-icon name="event" /></q-btn>
            <q-btn flat>5:30PM</q-btn>
            <q-btn flat>7:30PM</q-btn>
            <q-btn flat>9:00PM</q-btn>
            <q-btn flat color="primary">Reserve</q-btn>
          </q-card-actions>
        </q-card>
      </div>
    </q-pull-to-refresh>
  </div>
</template>

<script>
import * as firebase from 'firebase'
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
  QPullToRefresh,
  QProgress
} from 'quasar'

export default {
  name: 'profile',
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
    QPullToRefresh,
    QProgress
  },
  data () {
    return {
      stars: 4,
      filename: 'profile.jpg',
      progressUpload: 0,
      file: File,
      uploadTask: '',
      photoUrl: '',
      downloadURL: '',
      user: null
    }
  },
  methods: {
    refresher (done) {
      var user = firebase.auth().currentUser
      this.photoUrl = user.photoURL
      setTimeout(() => {
        done()
      }, 1000)
    },
    uploadPhoto (fileList) {
      Array.from(Array(fileList.length).keys()).map(x => {
        this.upload(fileList[x])
      })
    },
    upload (file) {
      var user = firebase.auth().currentUser
      this.uploadTask = firebase.storage().ref(user.uid + '/profilePicture/' + this.filename).put(file)
      this.uploadTask.then(snapshot => {
        this.downloadURL = this.uploadTask.snapshot.downloadURL
        this.$emit('url', this.downloadURL)
        user.updateProfile({
          displayName: 'Cheryl Dorgan',
          photoURL: this.downloadURL
        }).then(user => {
          this.photoURL = user.photoURL
        }).catch(function (error) {
          console.log(error)
        })
      })
    }
  },
  watch: {
    uploadTask: function () {
      this.uploadTask.on('state_changed', sp => {
        this.progressUpload = Math.floor(sp.bytesTransferred / sp.totalBytes * 100 | 0)
      })
    }
  },
  created () {
    var user = firebase.auth().currentUser
    if (user) {
      this.user = user
      this.photoUrl = user.photoURL
    }
    else {
      console.log('boo')
    }
  }
}
</script>

<style scoped>
img {
    margin: auto
  /* border-radius: 50% */
}
.custom-file-upload {
    border: 1px solid #ccc;
    display: inline-block;
    padding: 6px 12px;
    cursor: pointer;
}

input[type="file"] {
    display: none;
}
</style>

