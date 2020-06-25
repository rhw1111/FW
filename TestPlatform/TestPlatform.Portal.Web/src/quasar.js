import Vue from 'vue'

import './styles/quasar.scss'
import 'quasar/dist/quasar.ie.polyfills'
import '@quasar/extras/material-icons/material-icons.css'
import {
  // 组件
  Quasar,
  QLayout,
  QHeader,
  QDrawer,
  QPageContainer,
  QPage,
  QToolbar,
  QToolbarTitle,
  QBtn,
  QIcon,
  QList,
  QItem,
  QItemSection,
  QItemLabel,
  QFab,
  QFabAction,
  QTabs,
  QAvatar,
  QRouteTab,
  QDialog,
  QCard,
  QCardSection,
  QSeparator,
  QCardActions,
  QCheckbox,
  QInput,
  QRadio,
  // 事件
  Ripple,
  ClosePopup,
} from 'quasar'

Vue.use(Quasar, {
  config: {},
  components: {
    QLayout,
    QHeader,
    QDrawer,
    QPageContainer,
    QPage,
    QToolbar,
    QToolbarTitle,
    QBtn,
    QIcon,
    QList,
    QItem,
    QItemSection,
    QItemLabel,
    QFab,
    QFabAction,
    QAvatar,
    QTabs,
    QRouteTab,
    QDialog,
    QCard,
    QCardSection,
    QSeparator,
    QCardActions,
    QCheckbox,
    QInput,
    QRadio,
  },
  directives: {
    Ripple,
    ClosePopup,
  },
  plugins: {
  }
})